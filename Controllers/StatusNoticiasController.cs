using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class StatusNoticiasController{
    [HttpGet]
    public string Get(){
        return "Olá Notícias";
    }
}