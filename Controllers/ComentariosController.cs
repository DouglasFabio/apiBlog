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
    public async Task<ActionResult<IEnumerable<TbUsuario>>> Get()
    {
        try
        {   
            return Ok(await context.TbUsuarios.Where(p=> p.TipoUsuario == "A").ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter autores");
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

            autor.Email = autor.Email;
            autor.Senha = autor.Senha;
            
            context.TbUsuarios
                        .Where(u => u.Idusuario == id)
                        .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Nome, model.Nome)
                        );

            await context.SaveChangesAsync();
            return Ok("Nome editado com sucesso!");
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
            TbUsuario model = await context.TbUsuarios.FindAsync(id);

            if (model == null)
                return NotFound();

        //  SE  NA TABELA DE NOTICIAS, O CÓDIGO DO AUTOR FOR IGUAL O CÓDIGO CLICADO - FALSO 
            if(context.TbNoticias.Where(p => p.CodautorNavigation.Idusuario == id).Any() == false)
            {
                context.TbUsuarios.Remove(model);
                await context.SaveChangesAsync();
                return Ok("Autor deletado com sucesso!");
            }else
            {
                return BadRequest("Existem notícias vinculadas a este autor!");
            }
        }
        catch
        {
            return BadRequest("Erro ao remover autor.");
        }
    }
}