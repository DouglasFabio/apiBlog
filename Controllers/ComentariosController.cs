using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class ComentariosController : ControllerBase
{
    private readonly SeBlogContext context;

    public ComentariosController(SeBlogContext Context)
    {
        context = Context;
    }

[HttpPost]
    public async Task<ActionResult> Post([FromBody] TbStatusNoticia model)
    {
        try
        {
                model.DtComentario = DateTime.Now;
                context.TbStatusNoticias.Add(model);
                await context.SaveChangesAsync();
                return Ok("Comentário registrado com sucesso!");
            
        }
        catch
        {
            return BadRequest("Falha ao comentar.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbStatusNoticia>>> Get()
    {
        try
        {   
            return Ok(await context.TbStatusNoticias.ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter comentários");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TbStatusNoticia>> Get([FromRoute] int id)
    {
        try
        {
            if (await context.TbStatusNoticias.AnyAsync(p => p.IdstatusNoticia == id))
                return Ok(await context.TbStatusNoticias.FindAsync(id));
            else
                return NotFound();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            TbStatusNoticia model = await context.TbStatusNoticias.FindAsync(id);

            if (model == null)
                return NotFound();

            context.TbStatusNoticias.Remove(model);
            await context.SaveChangesAsync();
            return Ok("Comentário deletado com sucesso!");
        }
        catch
        {
            return BadRequest("Erro ao remover comentário.");
        }
    }
}