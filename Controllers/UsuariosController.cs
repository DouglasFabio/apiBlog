using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly DataContext context;

    public UsuariosController(DataContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Usuarios model)
    {
        try
        {
            context.TB_Usuarios.Add(model);
            await context.SaveChangesAsync();
            return Ok("Usuário cadastrado com sucesso!");
        }
        catch
        {
            return BadRequest("Falha ao cadastrar usuário.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuarios>>> Get()
    {
        try
        {
            return Ok(await context.TB_Usuarios.ToListAsync());
        }
        catch
        {
            return BadRequest("Erro ao obter usuários.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuarios>> Get([FromRoute] int id)
    {
        try
        {
            if (await context.TB_Usuarios.AnyAsync(p => p.IDUsuario == id))
                return Ok(await context.TB_Usuarios.FindAsync(id));
            else
                return NotFound();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromRoute] int id, [FromBody] Usuarios model)
    {
        if (id != model.IDUsuario)
            return BadRequest();

        try
        {
            if (await context.TB_Usuarios.AnyAsync(p => p.IDUsuario == id) == false)
                return NotFound();

            context.TB_Usuarios.Update(model);
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
            Usuarios model = await context.TB_Usuarios.FindAsync(id);

            if (model == null)
                return NotFound();

            context.TB_Usuarios.Remove(model);
            await context.SaveChangesAsync();
            return Ok("Usuário removido com sucesso");
        }
        catch
        {
            return BadRequest("Falha ao remover usuário.");
        }
    }
}