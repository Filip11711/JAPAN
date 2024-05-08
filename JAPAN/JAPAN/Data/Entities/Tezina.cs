using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Tezina
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Ispit> Ispiti { get; set; } = new List<Ispit>();

    public virtual ICollection<Tecaj> Tecaji { get; set; } = new List<Tecaj>();
}
