using System.ComponentModel.DataAnnotations;

public class StatusNoticias{
    [Required]
    public int IDStatusNoticia {get;set;}

    public int StatusNoticia {get;set;}

    [Required(ErrorMessage = "O Comentário é obrigatório")]
    public string Comentário {get;set;}

    [Required(ErrorMessage ="Código em branco")]
    public int CODNoticia {get;set;}

    [Required(ErrorMessage = "Código em branco")]
    public int CODLeitor {get;set;}

}