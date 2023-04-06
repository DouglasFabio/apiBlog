using apiBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class NoticiasController : ControllerBase
{
    private readonly SeBlogContext context;

    public NoticiasController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbNoticia model)
    {
        try
        {
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
            return Ok(await context.TbNoticias.ToListAsync());
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
        if (id != model.Idnoticia)
            return BadRequest();

        try
        {
            if (await context.TbNoticias.AnyAsync(p => p.Idnoticia == id) == false)
                return NotFound();

            context.TbNoticias.Update(model);
            await context.SaveChangesAsync();
            return Ok("Notícia salva com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }
}