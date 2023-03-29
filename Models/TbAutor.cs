using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbAutor
{
    public int Idautor { get; set; }

    public string ApelidoAutor { get; set; } = null!;

    public string SenhaProvisoria { get; set; } = null!;

    public int? Codusuario { get; set; }

    public virtual TbUsuario? CodusuarioNavigation { get; set; }

    public virtual ICollection<TbNoticia> TbNoticia { get; } = new List<TbNoticia>();
}
