using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbAutore
{
    public int Idautor { get; set; }

    public string? ApelidoAutor { get; set; }

    public string? SenhaProvisoria { get; set; }

    public int? Codusuario { get; set; }

    public virtual TbUsuario? CodusuarioNavigation { get; set; }

    public virtual ICollection<TbNoticia> TbNoticia { get; } = new List<TbNoticia>();
}
