using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PublicarNoticiaController : ControllerBase
{
    private readonly SeBlogContext context;

    public PublicarNoticiaController(SeBlogContext Context)
    {
        context = Context;
    }

[HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] TbNoticia model)
    {

        if (id != model.Idnoticia)
            return BadRequest();

        try
        {
            if (await context.TbNoticias.AnyAsync(p => p.Idnoticia == id) == false)
                return NotFound();
            

            TbNoticia noticia = new TbNoticia();

            noticia.Titulo = noticia.Titulo;
            noticia.Subtitulo = noticia.Subtitulo;
            noticia.Texto = noticia.Texto;

                context.TbNoticias
                    .Where(u => u.Idnoticia == id)
                    .ExecuteUpdate(s =>
                        s.SetProperty(u => u.Situacao, "P")
                    );
                await context.SaveChangesAsync();
                return Ok("Notícia publicada com sucesso!");          
        }
        catch
        {
            return BadRequest();
        }
    }
}