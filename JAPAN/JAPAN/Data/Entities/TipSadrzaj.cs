using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class TipSadrzaj
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Tecaj> Tecaji { get; set; } = new List<Tecaj>();
}
