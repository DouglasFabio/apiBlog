using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AutoresController{
    [HttpGet]
    public string Get(){
        return "Olá Autores";
    }
}