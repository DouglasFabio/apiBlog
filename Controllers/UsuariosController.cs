using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController{
    [HttpGet]
    public string Get(){
        return "Olá Usuários";
    }
}