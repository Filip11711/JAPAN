﻿using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Uloga
{
    public int Id { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Korisnik> Korisniks { get; set; } = new List<Korisnik>();
}
