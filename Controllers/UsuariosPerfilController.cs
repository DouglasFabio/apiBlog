using apiBlog.Models;
namespace apiBlog.Services;

using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsuariosPerfilController : ControllerBase
{
    private readonly SeBlogContext context;

    public UsuariosPerfilController(SeBlogContext Context)
    {
        context = Context;
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
             if(await context.TbUsuarios.AnyAsync(p => p.Email == model.Email)){
                return BadRequest("Email já utilizado!");
            }else{
                TbUsuario usuario = new TbUsuario();

                usuario.Nome = usuario.Nome;
                usuario.Senha = usuario.Senha;
                usuario.CodAtivacao = "U";
                usuario.CodAtivacao.GerarCodigo();

                context.TbUsuarios
                    .Where(u => u.Idusuario == id)
                    .ExecuteUpdate(s =>
                        s.SetProperty(u => u.Email, model.Email)
                            .SetProperty(u => u.Dtnascimento, model.Dtnascimento)
                    );
                
                MailMessage mail = new MailMessage();
                // adm_seblog@yahoo.com   // adm_seblog@outlook.com
                // Admin@seblog123
                //smtp.mail.yahoo.com       // SMTP.office365.com
                var d = "adm_seblog@outlook.com";
                var s = "Admin@seblog123";
                mail.From = new MailAddress(d);
                mail.To.Add(model.Email);
                mail.Subject = "ALTERAÇÃO DE EMAIL - StringElements Blog";
                mail.Body = "Olá "+usuario.Nome+", segue código de confirmação de novo email para verificação da sua conta no StringElements Blog: "+ usuario.CodAtivacao+"";

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
                        return BadRequest(ex);
                    }
                }

                await context.SaveChangesAsync();
                return Ok("Perfil editado com sucesso!");
            }       
        }
        catch
        {
            return BadRequest();
        }
    }
}