using apiBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class NoticiasPublicadasController : ControllerBase
{
    private readonly SeBlogContext context;

    public NoticiasPublicadasController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbNoticia model)
    {
        model.DataPublicacao = DateTime.Now;
        model.Situacao = "N";
        try
        {          
            context.TbNoticias
                    .Where(u => u.Idnoticia == model.Idnoticia)
                    .ExecuteUpdate(c =>
                        c.SetProperty(u => u.DataPublicacao, model.DataPublicacao)
                         .SetProperty(u => u.Situacao, model.Situacao)
                    );

            context.TbNoticias.Add(model);
            await context.SaveChangesAsync();
            return Ok("Notícia cadastrada com sucesso!");
        }
        catch
        {
            return BadRequest("Falha ao cadastrar notícia.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbNoticia>>> Get()
    {
        try
        {   
            return Ok(await context.TbNoticias.Where(p=> p.Situacao == "P").OrderByDescending(p => p.DataPublicacao).ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter notícias.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TbNoticia>> Get([FromRoute] int id)
    {
        try
        {
            if (await context.TbNoticias.AnyAsync(p => p.Idnoticia == id))
                return Ok(await context.TbNoticias.FindAsync(id));
            else
                return NotFound();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] TbNoticia model)
    {
        var dadosNoticia = context.TbNoticias.Where(p => p.Idnoticia == id);

        if (id != model.Idnoticia)
            return BadRequest();

        try
        {
            if (await context.TbNoticias.AnyAsync(p => p.Idnoticia == id) == false)
                return NotFound();
            

            TbNoticia noticia = new TbNoticia();

            noticia.DataPublicacao = noticia.DataPublicacao;
            noticia.DataAlteracao = DateTime.Now;
        
            context.TbNoticias
                        .Where(u => u.Idnoticia == id)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Titulo, model.Titulo)
                             .SetProperty(u => u.Subtitulo, model.Subtitulo)
                             .SetProperty(u => u.Texto, model.Texto)
                        );

            await context.SaveChangesAsync();
            return Ok("Notícia editada com sucesso!");
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
            TbNoticia model = await context.TbNoticias.FindAsync(id);

            if (model == null)
                return NotFound();

        //  SE  NA TABELA DE NOTICIAS, O CÓDIGO DO AUTOR FOR IGUAL O CÓDIGO CLICADO - FALSO 
                context.TbNoticias.Remove(model);
                await context.SaveChangesAsync();
                return Ok("Notícia deletada com sucesso!");
        }
        catch
        {
            return BadRequest("Erro ao remover notícia.");
        }
    }
}