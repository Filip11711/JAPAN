using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Ispit
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public string Opis { get; set; } = null!;

    public double Pozicija { get; set; }

    public int Idtezina { get; set; }

    public virtual Tezina Tezina { get; set; } = null!;

    public virtual ICollection<Statistika> Statistike { get; set; } = new List<Statistika>();

    public virtual ICollection<Pitanje> Pitanja { get; set; } = new List<Pitanje>();
}
