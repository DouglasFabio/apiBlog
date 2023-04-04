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

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        if (await context.TbUsuarios.AnyAsync(p => p.CodSenha != model.CodSenha))
            return BadRequest("Código Inválido, tente novamente.");
        else{
            try
            {
                if (await context.TbUsuarios.AnyAsync(p => p.Senha == model.Senha))
                    return BadRequest("Senha já utilizada!");
                else if (await context.TbUsuarios.AnyAsync(p => p.DtaltSenha < DateTime.Now)) 
                    return BadRequest("Código Expirado, solicite novamente.");
                
                string limpaCodSenha = "";
                model.Senha = model.Senha.GerarHash();

                context.TbUsuarios
                    .Where(u => u.Email == model.Email)
                    .ExecuteUpdate(s =>
                        s.SetProperty(u => u.Senha, model.Senha)
                         .SetProperty(u => u.CodSenha, limpaCodSenha)
                    );
                context.TbUsuarios.Add(model);
                context.SaveChanges();
                return Ok("Senha alterada com sucesso!");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}