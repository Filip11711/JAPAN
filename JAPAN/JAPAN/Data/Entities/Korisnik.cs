using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Korisnik
{
    public string Korisnickoime { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Id { get; set; }

    public int Iduloga { get; set; }

    public virtual ICollection<ForumOdgovor> ForumOdgovors { get; set; } = new List<ForumOdgovor>();

    public virtual ICollection<ForumPitanje> ForumPitanjes { get; set; } = new List<ForumPitanje>();

    public virtual Uloga IdulogaNavigation { get; set; } = null!;

    public virtual ICollection<Statistika> Statistikas { get; set; } = new List<Statistika>();
}
