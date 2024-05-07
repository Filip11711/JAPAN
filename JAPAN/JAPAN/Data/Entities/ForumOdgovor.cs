using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class ForumOdgovor
{
    public int Id { get; set; }

    public string Sadrzaj { get; set; } = null!;

    public DateOnly Kreirano { get; set; }

    public int Idkorisnik { get; set; }

    public int Idpitanje { get; set; }

    public virtual Korisnik IdkorisnikNavigation { get; set; } = null!;

    public virtual ForumPitanje IdpitanjeNavigation { get; set; } = null!;
}
