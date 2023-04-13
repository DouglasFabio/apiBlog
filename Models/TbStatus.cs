using System;
using System.Collections.Generic;

namespace apiBlog.Models;

public partial class TbStatus
{
    public int Idstatus { get; set; }

    public string NomeStatus { get; set; } = null!;

    public virtual ICollection<TbStatusNoticia> TbStatusNoticia { get; } = new List<TbStatusNoticia>();
}
