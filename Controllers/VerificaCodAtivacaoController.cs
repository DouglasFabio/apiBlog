using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class VerificaCodAtivacaoController : ControllerBase
{
    private readonly SeBlogContext context;

    public VerificaCodAtivacaoController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] TbUsuario model)
    {
        if (await context.TbUsuarios.AnyAsync(p => p.CodAtivacao == model.CodAtivacao) == false)
            return BadRequest("Código inválido, tente novamente.");
        
        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.CodAtivacao == model.CodAtivacao)){
                TbUsuario usuario = new TbUsuario();
                usuario.Nome = usuario.Nome;
                usuario.Email = usuario.Email;
                usuario.Senha = usuario.Senha;
                usuario.StatusConta = "V";

                context.TbUsuarios
                        .Where(u => u.CodAtivacao == model.CodAtivacao)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.StatusConta, usuario.StatusConta)
                        );
                
                await context.SaveChangesAsync();
                
                usuario.CodAtivacao = null;
                context.TbUsuarios
                        .Where(u => u.CodAtivacao == model.CodAtivacao)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.CodAtivacao, usuario.CodAtivacao)
                        );

                return Ok("Conta verificada com sucesso!");
            }else 
                return BadRequest("Não foi possível verificar conta.");        
        }
        catch
        {
            return BadRequest();
        }
    }
}