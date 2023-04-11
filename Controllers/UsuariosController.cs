using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly SeBlogContext context;

    public UsuariosController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        try
        {
            //Se o email digitado já existir no SE-Blog
            if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email))
                return BadRequest("Email já cadastrado!");

            //Se ADM já existir no SE-Blog
            if (await context.TbUsuarios.Where(p => p.TipoUsuario == model.TipoUsuario).AnyAsync(p => p.TipoUsuario == "M"))
                return BadRequest("Já existe um administrador do sistema!");
        
            else{

                if (model.TipoUsuario == "L"){
                    model.CodAtivacao = model.CodAtivacao.GerarCodigo();
                    model.StatusConta = "N";
                    model.StatusSenha = "N";
                    model.Senha = model.Senha.GerarHash();

                    context.TbUsuarios
                        .Where(u => u.Email == model.Email)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.CodAtivacao, model.CodAtivacao)
                        );

                    MailMessage mail = new MailMessage();
                    // adm_seblog@yahoo.com   // adm_seblog@outlook.com
                    // Admin@seblog123
                    //smtp.mail.yahoo.com       // SMTP.office365.com
                    var d = "adm_seblog@outlook.com";
                    var s = "Admin@seblog123";
                    mail.From = new MailAddress(d);
                    mail.To.Add(model.Email);
                    mail.Subject = "CÓDIGO DE ATIVAÇÃO - StringElements Blog";
                    mail.Body = "Olá "+model.Nome+", segue código de ativação para verificação da sua conta no StringElements Blog: "+ model.CodAtivacao+"";

                    using (var smtp = new SmtpClient("SMTP.office365.com", 587)){
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(d,s);
                        
                        try
                        {
                            smtp.Send(mail);
                        }
                        catch (System.Exception ex)
                        {
                            return BadRequest(ex);
                        }
                    }
                
                }else if(model.TipoUsuario == "A"){
                    model.CodAtivacao = null;
                    model.StatusConta = "N";
                    model.StatusSenha = "N";
                    model.Senha = model.Senha.GerarCodigo();

                    MailMessage mail = new MailMessage();
                    // stringelements@myyahoo.com
                    // Admin@seblog
                    //smtp.mail.yahoo.com
                    var d = "adm_seblog@outlook.com";
                    var s = "Admin@seblog";
                    mail.From = new MailAddress(d);
                    mail.To.Add(model.Email);
                    mail.Subject = "SENHA TEMPORÁRIA - StringElements Blog";
                    mail.Body = "Olá "+model.Nome+", segue senha de primeiro acesso ao StringElements Blog: "+ model.Senha+"";

                    using (var smtp = new SmtpClient("SMTP.office365.com", 587)){
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(d,s);
                        
                        try
                        {
                            smtp.Send(mail);
                        }
                        catch (System.Exception ex)
                        {
                            return BadRequest(ex);
                        }
                    }    
                }else if(model.TipoUsuario == "M"){
                    model.CodAtivacao = null;
                    model.TipoUsuario = "M";
                    model.StatusConta = "V";
                    model.StatusSenha = "V";
                    model.Senha = model.Senha.GerarHash();
                }else{
                    return BadRequest("Impossível cadastrar usuário");
                }
              
                context.TbUsuarios.Add(model);
                await context.SaveChangesAsync();
                return Ok("Usuário cadastrado com sucesso!");
            }
        }
        catch
        {
            return BadRequest("Falha ao cadastrar usuário.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbUsuario>>> Get()
    {
        try
        {   
            return Ok(await context.TbUsuarios.ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter usuários.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TbUsuario>> Get([FromRoute] int id)
    {
        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Idusuario == id))
                return Ok(await context.TbUsuarios.FindAsync(id));
            else
                return NotFound();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] TbUsuario model)
    {
        var dadosAutor = context.TbUsuarios.Where(p => p.Idusuario == id);

        if (id != model.Idusuario)
            return BadRequest();

        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Idusuario == id) == false)
                return NotFound();
            

            TbUsuario autor = new TbUsuario();

            autor.Nome = autor.Nome;
            autor.Senha = autor.Senha;
            
            context.TbUsuarios
                        .Where(u => u.Idusuario == id)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Email, model.Email)
                             .SetProperty(v => v.Dtnascimento , model.Dtnascimento)
                        );

            await context.SaveChangesAsync();
            return Ok("Dados atualizados com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }

    [AllowAnonymous]
    [HttpPost("autenticar")]
    public async Task<ActionResult> Autenticar([FromBody] TbUsuario usuario)
    {
        try
        {
            TbUsuario autenticado = await context.TbUsuarios.FirstOrDefaultAsync(p => p.Email == usuario.Email && p.Senha == usuario.Senha);

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

            if (autenticado.Email.EndsWith("@ifsp.edu.br"))
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            usuario.Token = tokenHandler.WriteToken(token);

            return Ok(usuario.Token);
        }
        catch
        {
            return BadRequest();
        }
    }
}