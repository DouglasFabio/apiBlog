using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class AutoresController : ControllerBase
{
    private readonly SeBlogContext context;

    public AutoresController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        try
        {
            //Se o email digitado já existir no SE-Blog
            if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email))
                return BadRequest("Email já cadastrado!");
            else{
                
                TbAutore autor = new TbAutore();
                autor.ApelidoAutor = model.Nome;
                model.Senha = model.Senha.GerarCodigo();
                autor.SenhaProvisoria = model.Senha;

              
                model.CodAtivacao = null;

                MailMessage mail = new MailMessage();
                var d = "adm_seblog@outlook.com";
                var s = "Admin@seblog";
                mail.From = new MailAddress(d);
                mail.To.Add(model.Email);
                mail.Subject = "SENHA PROVISÓRIA - StringElements Blog";
                mail.Body = "Olá "+model.Nome+", segue senha provisória de acesso ao StringElements Blog: "+ autor.SenhaProvisoria+"";

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
                context.TbUsuarios.Add(model);
                context.TbAutores
                .Where(a => a.Codusuario == model.Idusuario)
                .ExecuteUpdate(s =>
                    s.SetProperty(u => u.Codusuario, model.Idusuario)
                );
                context.TbAutores.Add(autor);
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
            return Ok(await context.TbAutores.ToListAsync());
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

            context.TbUsuarios
                .Where(u => u.Idusuario == id)
                .ExecuteUpdate(s =>
                    s.SetProperty(u => u.Email, model.Email)
                     .SetProperty(u => u.Dtnascimento, model.Dtnascimento)
                );
            await context.SaveChangesAsync();
            return Ok("Dados atualizados com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }
}