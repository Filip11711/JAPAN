using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Tezina
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Ispit> Ispits { get; set; } = new List<Ispit>();

    public virtual ICollection<Tecaj> Tecajs { get; set; } = new List<Tecaj>();
}
