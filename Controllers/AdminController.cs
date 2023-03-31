using apiBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly SeBlogContext context;

    public AdminController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        try
        {
            context.TbUsuarios.Add(model);
            await context.SaveChangesAsync();
            return Ok("Usuário cadastrado com sucesso!");
        }
        catch
        {
            return BadRequest("Falha ao cadastrar usuário.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbUsuario>>> Get()
    {
        try
        {   
            return Ok(await context.TbUsuarios.ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter usuários.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TbUsuario>> Get([FromRoute] int id)
    {
        try
        {
            if (await context.TbUsuarios.AnyAsync(t => t.Idusuario == id))
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
        if (id != model.Idusuario)
            return BadRequest();

        try
        {
            if (await context.TbUsuarios.AnyAsync(t => t.Idusuario == id) == false)
                return NotFound();

            context.TbUsuarios.Update(model);
            await context.SaveChangesAsync();
            return Ok("Usuário salvo com sucesso!");
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

            context.TbUsuarios.Remove(model);
            await context.SaveChangesAsync();
            return Ok("Usuário removido com sucesso");
        }
        catch
        {
            return BadRequest("Falha ao remover usuário.");
        }
    }
}