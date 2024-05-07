using System;
using System.Collections.Generic;

namespace JAPAN.Data.Entities;

public partial class Odgovor
{
    public int Id { get; set; }

    public string Tekst { get; set; } = null!;

    public int Tocno { get; set; }

    public int Idpitanje { get; set; }

    public virtual Pitanje IdpitanjeNavigation { get; set; } = null!;
}
