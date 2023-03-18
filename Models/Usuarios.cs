using System.ComponentModel.DataAnnotations;
public class Usuarios{
    [Required]
    public int IDUsuario {get;set;}

    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(50, ErrorMessage = "O nome deve possuir, no máximo, 50 caracteres")]
    public string Nome {get;set;}

    [Required(ErrorMessage = "O Email é obrigatório")]
    [MaxLength(50, ErrorMessage = "O nome deve possuir, no máximo, 50 caracteres")]
    public string Email {get;set;}

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha {get;set;}

    [Required(ErrorMessage ="A data de nascimento é obrigatória")]
    [Range(typeof(DateTime), "01-12-2022", "31-12-2022")]
    public DateTime DataNascimento {get;set;}

    [Required(ErrorMessage = "Código em branco")]
    public string CodAtivacao {get;set;}

    [Required(ErrorMessage = "Status em branco")]
    [MaxLength(1, ErrorMessage = "Status inválido")]
    public string StatusSenha {get;set;}

    [Required(ErrorMessage = "Status em branco")]
    [MaxLength(1, ErrorMessage = "Status inválido")]
    public string StatusConta {get;set;}

    [Required(ErrorMessage = "Tipo em branco")]
    [MaxLength(1, ErrorMessage = "Tipo inválido")]
    public string TipoUsuario {get;set;}
}