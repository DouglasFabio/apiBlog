using System.ComponentModel.DataAnnotations;

public class Admin{
    
    [Required]
    public int IDAdmin {get;set;}

    [Required(ErrorMessage = "O Nome é obrigatório")]
    [MaxLength(50, ErrorMessage = "O apelido deve possuir, no máximo, 50 caracteres")]
    public string NomeAdmin {get;set;}

    [Required(ErrorMessage = "O Email é obrigatório")]
    [MaxLength(50, ErrorMessage = "O Email deve possuir, no máximo, 50 caracteres")]
    public string EmailAdmin {get;set;}

    [Required(ErrorMessage = "A Senha é obrigatória")]
    public string SenhaAdmin {get;set;}
}