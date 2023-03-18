using System.ComponentModel.DataAnnotations;

public class Noticias{

    [Required]
    public int IDNoticia {get;set;}

    [Required(ErrorMessage = "O título é obrigatório")]
    [MaxLength(25, ErrorMessage = "O título deve possuir, no máximo, 25 caracteres")]
    public string Titulo {get;set;}

    [Required(ErrorMessage = "O Subtítulo é obrigatório")]
    [MaxLength(50, ErrorMessage = "O Subtítulo deve possuir, no máximo, 50 caracteres")]
    public string SubTitulo {get;set;}

    [Required(ErrorMessage ="A data de publicação é obrigatória")]
    [Range(typeof(DateTime), "01-12-2022", "31-12-2022")]
    public DateTime DataPublicacao {get;set;}

    [Required(ErrorMessage = "O Texto é obrigatório")]
    public string Texto {get;set;}

    [Required(ErrorMessage = "Situação em branco")]
    [MaxLength(1, ErrorMessage = "Situação inválida")]
    public string Situacao {get;set;}

    [Required(ErrorMessage ="A data de alteração é obrigatória")]
    [Range(typeof(DateTime), "01-12-2022", "31-12-2022")]
    public DateTime DataAlteracao {get;set;}

    [Required(ErrorMessage = "Código em branco")]
    public string CODAutor {get;set;}
}