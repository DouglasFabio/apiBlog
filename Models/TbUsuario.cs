using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbUsuario
{
    public int IdUsuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public DateTime? DtNascimento { get; set; }

    public string? CodAtivacao { get; set; } 

    public string? StatusSenha { get; set; } 

    public string? StatusConta { get; set; } 

    public string? TipoUsuario { get; set; } 

    public virtual ICollection<TbAutor> TbAutors { get; } = new List<TbAutor>();

    public virtual ICollection<TbStatusNoticia> TbStatusNoticia { get; } = new List<TbStatusNoticia>();
}
