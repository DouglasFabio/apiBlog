using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbNoticia
{
    public int Idnoticia { get; set; }

    public string Titulo { get; set; } = null!;

    public string Subtitulo { get; set; } = null!;

    public DateTime? DataPublicacao { get; set; }

    public string Texto { get; set; } = null!;

    public string Situacao { get; set; } = null!;

    public DateTime? DataAlteracao { get; set; }

    public int? Codautor { get; set; }

    public virtual TbUsuario? CodautorNavigation { get; set; }

    public virtual ICollection<TbStatusNoticia> TbStatusNoticia { get; } = new List<TbStatusNoticia>();
}
