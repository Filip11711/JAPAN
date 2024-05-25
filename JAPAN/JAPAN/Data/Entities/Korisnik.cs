using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Korisnik
{
    public string Korisnickoime { get; set; } = null!;

    public string Identifikator { get; set; } = null!;

    public int Id { get; set; }

    public string? Email { get; set; } = null!;

    public string? Ime { get; set; } = null!;

    public string? Prezime { get; set; } = null!;

    public DateOnly? DatumRodenja { get; set; }

    public string? Preporuka { get; set; } = "Početnik";

    public int Iduloga { get; set; }

    public virtual ICollection<ForumOdgovor> ForumOdgovori { get; set; } = new List<ForumOdgovor>();

    public virtual ICollection<ForumPitanje> ForumPitanja { get; set; } = new List<ForumPitanje>();

    public virtual Uloga Uloga { get; set; } = null!;

    public virtual ICollection<Statistika> Statistike { get; set; } = new List<Statistika>();
}
