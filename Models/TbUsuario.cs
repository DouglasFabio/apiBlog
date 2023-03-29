using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbUsuario
{
    public int IdUsuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string? DtNascimento { get; set; }

    public string CodAtivacao { get; set; } = null!;

    public string StatusSenha { get; set; } = null!;

    public string StatusConta { get; set; } = null!;

    public string TipoUsuario { get; set; } = null!;

    public virtual ICollection<TbAutor> TbAutors { get; } = new List<TbAutor>();

    public virtual ICollection<TbStatusNoticia> TbStatusNoticia { get; } = new List<TbStatusNoticia>();
}
