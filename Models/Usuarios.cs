using System.ComponentModel.DataAnnotations;

public class Usuarios{
    public int IDUsuario {get;set;}

    [Required(ErrorMessage = "O campo NOME é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O nome deve possuir, no máximo, 50 caracteres")]
    public string Nome {get;set;} = null!;

    [Required(ErrorMessage = "O campo EMAIL é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O email deve possuir, no máximo, 50 caracteres")]
    public string Email {get;set;} = null!;

    [Required(ErrorMessage = "O campo SENHA é obrigatório!")]
    [MinLength(8)]
    public string Senha {get;set;} = null!;

    [Range(typeof(DateTime), "01-01-1920", "31-12-2022")]
    public DateTime DTNascimento {get;set;}

    [Required(ErrorMessage = "Código em branco!")]
    public string CodAtivacao {get;set;} = null!;

    [Required(ErrorMessage = "Status em branco!")]
    [MaxLength(1, ErrorMessage = "Status da Senha inválido!")]
    public string StatusSenha {get;set;} = null!;

    [Required(ErrorMessage = "Status em branco!")]
    [MaxLength(1, ErrorMessage = "Status da Conta inválido!")]
    public string StatusConta {get;set;} = null!;

    [Required(ErrorMessage = "Tipo em branco!")]
    [MaxLength(1, ErrorMessage = "Tipo de Usuário inválido!")]
    public string TipoUsuario {get;set;} = null!;
    
    public virtual ICollection<Autores> Autores { get; } = new List<Autores>();

    public virtual ICollection<StatusNoticias> TbstatusNoticia { get; } = new List<StatusNoticias>();
}