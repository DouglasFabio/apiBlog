using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class EnviaCodSenhaController : ControllerBase
{
    private readonly SeBlogContext context;

    public EnviaCodSenhaController(SeBlogContext Context)
    {
        context = Context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] TbUsuario model)
    {
        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email ) == false)
                return NotFound("Usuário não cadastrado!");
            else{
                
                var usuario = context.TbUsuarios.Where(p => p.Email == model.Email).FirstOrDefault();

                 model.CodSenha = model.CodSenha.GerarCodigo();

                MailMessage mail = new MailMessage();
                var d = "adm_seblog@outlook.com";
                var s = "Admin@seblog";
                mail.From = new MailAddress(d);
                mail.To.Add(model.Email);
                mail.Subject = "RESETAR SENHA - StringElements Blog";
                mail.Body = "Olá "+usuario.Nome+", utilize o código: "+ model.CodSenha+" para resetar a senha no StringElements Blog. Este código expira em 30 MINUTOS.";

                context.TbUsuarios
                    .Where(u => u.Email == model.Email)
                    .ExecuteUpdate(c =>
                        c.SetProperty(u => u.CodSenha, model.CodSenha)
                        .SetProperty(u => u.DtaltSenha, DateTime.Now.AddMinutes(30))
                    );
                
                //context.TbUsuarios.Attach(usuario);
                //context.Entry(usuario).Property(c => c.CodSenha).IsModified = true;
                context.SaveChanges();

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
            }
            return Ok("Código enviado, verifique seu email!");
        }
        catch
        {
            return BadRequest("Erro ao enviar código!");
        }
    }
}