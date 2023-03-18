using System.ComponentModel.DataAnnotations;

public class Autores{
    
    [Required]
    public int IDAutor {get;set;}

    [Required(ErrorMessage = "O Apelido é obrigatório")]
    [MaxLength(25, ErrorMessage = "O apelido deve possuir, no máximo, 25 caracteres")]
    public string Apelido {get;set;}

    [Required(ErrorMessage = "Senha provisória em branco")]
    public string SenhaProvisoria {get;set;}

    [Required(ErrorMessage = "Código inválido")]
    public int CODUsuario {get;set;}
}