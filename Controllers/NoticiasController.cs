using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NoticiasController{
    [HttpGet]
    public string Get(){
        return "Olá Notícias";
    }
}