using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Statistika
{
    public int Id { get; set; }

    public string? Rezultat { get; set; }

    public DateOnly Zavrseno { get; set; }

    public int Idkorisnik { get; set; }

    public int? Idtecaj { get; set; }

    public int? Idispit { get; set; }

    public virtual Ispit? Ispit { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Tecaj? Tecaj { get; set; }
}
