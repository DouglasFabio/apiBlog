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

            //Se ADM já existir no SE-Blog
            if (await context.TbUsuarios.Where(p => p.TipoUsuario == model.TipoUsuario).AnyAsync(p => p.TipoUsuario == "M"))
                return BadRequest("Já existe um administrador do sistema!");
        
            else{

                if(model.TipoUsuario == "A"){
                    model.CodAtivacao = null;
                    model.StatusConta = "N";
                    model.StatusSenha = "N";
                    model.Senha = model.Senha.GerarCodigo();

                    MailMessage mail = new MailMessage();
                    // stringelements@myyahoo.com
                    // Admin@seblog
                    //smtp.mail.yahoo.com
                    var d = "adm_seblog@outlook.com";
                    var s = "Admin@seblog123";
                    mail.From = new MailAddress(d);
                    mail.To.Add(model.Email);
                    mail.Subject = "SENHA TEMPORÁRIA - StringElements Blog";
                    mail.Body = "Olá "+model.Nome+", segue senha de primeiro acesso ao StringElements Blog: "+ model.Senha+"";

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
                            return BadRequest();
                        }
                    }    
                }else{
                    return BadRequest("Impossível cadastrar autor!");
                }
                context.TbUsuarios.Add(model);
                await context.SaveChangesAsync();
                return Ok("Autor cadastrado com sucesso!");
            }
        }
        catch
        {
            return BadRequest("Falha ao cadastrar autor.");
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
        if (id != model.Idusuario)
            return BadRequest();

        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Idusuario == id) == false)
                return NotFound();

            context.TbUsuarios.Update(model);
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