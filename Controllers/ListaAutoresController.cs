using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class ListaAutoresController : ControllerBase
{
    private readonly SeBlogContext context;

    public ListaAutoresController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbUsuario>>> Get()
    {
        try
        {   
            TbNoticia noticiaAutor = new TbNoticia();

            return Ok(await context.TbUsuarios.Where(p=> p.TipoUsuario == "A").Where(p => p.StatusSenha =="N").ToListAsync());
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

}