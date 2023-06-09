﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiBlog.Models;

public partial class TbUsuario
{
    public int Idusuario { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public DateTime? Dtnascimento { get; set; }

    public string? CodAtivacao { get; set; }

    public string? StatusSenha { get; set; }

    public string? StatusConta { get; set; }

    public string? TipoUsuario { get; set; }

    public string? CodSenha { get; set; }

    public DateTime? DtaltSenha { get; set; }

    public string? ApelidoAutor { get; set; }

    [NotMapped]
    public string? Token {get; set;}

    [NotMapped]
    public string? SenhaInicial {get; set;}

    public virtual ICollection<TbNoticia> TbNoticia { get; } = new List<TbNoticia>();

    public virtual ICollection<TbStatusNoticia> TbStatusNoticia { get; } = new List<TbStatusNoticia>();
}
