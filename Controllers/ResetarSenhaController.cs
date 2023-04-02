using apiBlog.Models;
namespace apiBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class ResetarSenhaController : ControllerBase
{
    private readonly SeBlogContext context;

    public ResetarSenhaController(SeBlogContext Context)
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
            else if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email)){
                MailMessage mail = new MailMessage();
                var d = "adm_seblog@outlook.com";
                var s = "Admin@seblog";
                mail.From = new MailAddress(d);
                mail.To.Add(model.Email);
                mail.Subject = "RESETAR SENHA - StringElements Blog";
                mail.Body = "Olá "+model.Nome+", utilize o código: "+ model.CodAtivacao+" para resetar a senha no StringElements Blog";

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
            return BadRequest("Email não encontrado.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] TbUsuario model)
    {
        if (await context.TbUsuarios.AnyAsync(p => p.CodSenha != model.CodSenha))
            return BadRequest("Código Inválido, tente novamente.");

        try
        {
            if (await context.TbUsuarios.AnyAsync(p => p.Email == model.Email) == false)
                return NotFound("Não foi possível alterar a senha.");

            context.TbUsuarios.Update(model);
            await context.SaveChangesAsync();
            return Ok("Senha alterada com sucesso!");
        }
        catch
        {
            return BadRequest();
        }
    }
}