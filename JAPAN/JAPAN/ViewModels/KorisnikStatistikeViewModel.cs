using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class KorisnikStatistikeViewModel
    {
        public List<Statistika> Statistike_tecaja { get; set; } = null!;
        public List<Statistika> Statistike_ispita { get; set; } = null!;
    }
}
