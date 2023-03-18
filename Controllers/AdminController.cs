using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AdminController{
    [HttpGet]
    public string Get(){
        return "Olá Administrador";
    }
}