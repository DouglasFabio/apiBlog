using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class VerificaSenhaInicialAutorController : ControllerBase
{
    private readonly SeBlogContext context;

    public VerificaSenhaInicialAutorController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] TbUsuario model)
    {
        if (await context.TbUsuarios.AnyAsync(p => p.Senha == model.SenhaInicial) == false)
            return BadRequest("Senha inválida, digite a senha inicial recebida no email.");
        
        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Senha == model.SenhaInicial)){
                
                TbUsuario autor = new TbUsuario();
                autor.Nome = autor.Nome;
                autor.Email = autor.Email;
                autor.Senha = model.Senha.GerarHash();
                autor.StatusConta = "V";

                context.TbUsuarios
                        .Where(u => u.Senha == model.SenhaInicial)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Senha, autor.Senha)
                             .SetProperty(u => u.StatusConta, autor.StatusConta)
                        );
                
                await context.SaveChangesAsync();
                return Ok("Senha atualizada com sucesso!");
            }else 
                return BadRequest("Não foi possível atualizar senha.");        
        }
        catch
        {
            return BadRequest();
        }
    }
}