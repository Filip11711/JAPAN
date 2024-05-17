using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class KorisnikTecajiViewModel
    {
        public List<Tecaj> Tecaji { get; set; } = null!;
        public Dictionary<int, string?> Rezultati { get; set; } = null!;
    }
}
