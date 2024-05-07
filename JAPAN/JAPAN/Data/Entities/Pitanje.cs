﻿using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Pitanje
{
    public int Id { get; set; }

    public string Tekst { get; set; } = null!;

    public virtual ICollection<Odgovor> Odgovors { get; set; } = new List<Odgovor>();

    public virtual ICollection<Ispit> Idispits { get; set; } = new List<Ispit>();
}
