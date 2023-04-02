using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly SeBlogContext context;

    public UsuariosController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email))
                return BadRequest("Email já cadastrado!");
            else{

                MailMessage mail = new MailMessage();
                var d = "adm_seblog@outlook.com";
                var s = "Admin@seblog";
                mail.From = new MailAddress(d);
                mail.To.Add(model.Email);
                mail.Subject = "CÓDIGO DE ATIVAÇÃO - StringElements Blog";
                mail.Body = "Olá "+model.Nome+", segue código de ativação para verificação da sua conta no StringElements Blog: "+ model.CodAtivacao+"";

                using (var smtp = new SmtpClient("SMTP.office365.com", 587)){
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(d,s);
                    
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (System.Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                model.Senha = model.Senha.GerarHash();

                context.TbUsuarios.Add(model);
                await context.SaveChangesAsync();
                return Ok("Usuário cadastrado com sucesso!");
            }
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
        if (id != model.Idusuario)
            return BadRequest();

        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Idusuario == id) == false)
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