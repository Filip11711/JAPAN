﻿using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Tecaj
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public string Opis { get; set; } = null!;

    public string Sadrzaj { get; set; } = null!;

    public DateOnly Kreirano { get; set; }

    public double Pozicija { get; set; }

    public int Idtipsadrzaj { get; set; }

    public int Idtezina { get; set; }

    public virtual Tezina Tezina { get; set; } = null!;

    public virtual TipSadrzaj Tipsadrzaja { get; set; } = null!;

    public virtual ICollection<Statistika> Statistike { get; set; } = new List<Statistika>();
}
