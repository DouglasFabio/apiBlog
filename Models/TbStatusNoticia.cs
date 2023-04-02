using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbStatusNoticia
{
    public int IdstatusNoticia { get; set; }

    public int StatusNoticia { get; set; }

    public string? Comentario { get; set; }

    public int? Codnoticia { get; set; }

    public int? Codleitor { get; set; }

    public DateTime? DtComentario { get; set; }

    public virtual TbUsuario? CodleitorNavigation { get; set; }

    public virtual TbNoticia? CodnoticiaNavigation { get; set; }
}
