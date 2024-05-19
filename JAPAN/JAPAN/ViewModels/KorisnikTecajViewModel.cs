using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class KorisnikTecajViewModel
    {
        public Korisnik Korisnik { get; set; } = null!;
        public Tecaj Tecaj { get; set; } = null!;
        public string? Rezultat {  get; set; }
    }
}
