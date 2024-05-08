using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class ForumPitanje
{
    public int Id { get; set; }

    public string Naslov { get; set; } = null!;

    public string Sadrzaj { get; set; } = null!;

    public DateOnly Kreirano { get; set; }

    public int Idkorisnik { get; set; }

    public virtual ICollection<ForumOdgovor> ForumOdgovori { get; set; } = new List<ForumOdgovor>();

    public virtual Korisnik Korisnik { get; set; } = null!;
}
