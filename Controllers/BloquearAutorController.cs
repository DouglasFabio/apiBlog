using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BloquearAutorController : ControllerBase
{
    private readonly SeBlogContext context;

    public BloquearAutorController(SeBlogContext Context)
    {
        context = Context;
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
            autor.Email = autor.Email;
            autor.Senha = autor.Senha;
               
            context.TbUsuarios
                        .Where(u => u.Idusuario == id)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.StatusConta, "B")
                        );

            await context.SaveChangesAsync();
            return Ok("Conta bloqueada com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }
}