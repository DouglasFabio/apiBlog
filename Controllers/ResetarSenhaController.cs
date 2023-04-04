using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ResetarSenhaController : ControllerBase
{
    private readonly SeBlogContext context;

    public ResetarSenhaController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] TbUsuario model)
    {
        if (await context.TbUsuarios.AnyAsync(p => p.CodSenha == model.CodSenha) == false)
            return BadRequest("Código Incorreto, tente novamente.");
        try
        {
            model.Senha = model.Senha.GerarHash();
            if (await context.TbUsuarios.AnyAsync(p => p.Senha == model.Senha))
                return BadRequest("Senha já utilizada!");
            else if (await context.TbUsuarios.AnyAsync(p => p.DtaltSenha < DateTime.Now)) 
                return BadRequest("Código Expirado, solicite novamente.");
            
            context.TbUsuarios.Add(model);
            await context.SaveChangesAsync();
            return Ok("Senha alterada com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }
}