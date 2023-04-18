using apiBlog.Models;
namespace apiBlog.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class AutenticarController : ControllerBase
{
    private readonly SeBlogContext context;

    public AutenticarController(SeBlogContext Context)
    {
        context = Context;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Autenticar([FromBody] TbUsuario usuario)
    {
        try
        {
            TbUsuario autenticado = await context.TbUsuarios.FirstOrDefaultAsync(p => p.Email == usuario.Email && p.Senha == usuario.Senha.GerarHash());

            if (autenticado == null)
                return BadRequest("Usuário inválido");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetValue("Secret", ""));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Sid, autenticado.Idusuario.ToString()),
                new Claim(ClaimTypes.Name, autenticado.Nome),
                new Claim(ClaimTypes.Email, autenticado.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            if (autenticado.TipoUsuario.Equals("M"))
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Master"));
            if (autenticado.TipoUsuario.Equals("A"))
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Author"));
            if (autenticado.TipoUsuario.Equals("R"))
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Reader"));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            usuario.Token = tokenHandler.WriteToken(token);

            return Ok("Autenticado.");
        }
        catch
        {
            return BadRequest();
        }
    }
}