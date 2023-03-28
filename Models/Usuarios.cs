using System.ComponentModel.DataAnnotations;
public class Usuarios{
    public int IDUsuario {get;set;}

    [Required(ErrorMessage = "O campo NOME é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O nome deve possuir, no máximo, 50 caracteres")]
    public string Nome {get;set;}

    [Required(ErrorMessage = "O campo EMAIL é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O email deve possuir, no máximo, 50 caracteres")]
    public string Email {get;set;}

    [Required(ErrorMessage = "O campo SENHA é obrigatório!")]
    [MinLength(8)]
    public string Senha {get;set;}

    [Required(ErrorMessage ="O campo DATA DE NASCIMENTO é obrigatório!")]
    [Range(typeof(DateTime), "01-12-2022", "31-12-2022")]
    public DateTime DTNascimento {get;set;}

    [Required(ErrorMessage = "Código em branco!")]
    public string CodAtivacao {get;set;}

    [Required(ErrorMessage = "Status em branco!")]
    [MaxLength(1, ErrorMessage = "Status da Senha inválido!")]
    public string StatusSenha {get;set;}

    [Required(ErrorMessage = "Status em branco!")]
    [MaxLength(1, ErrorMessage = "Status da Conta inválido!")]
    public string StatusConta {get;set;}

    [Required(ErrorMessage = "Tipo em branco!")]
    [MaxLength(1, ErrorMessage = "Tipo de Usuário inválido!")]
    public string TipoUsuario {get;set;}
}