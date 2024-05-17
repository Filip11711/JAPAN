using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class KorisnikIspitiViewModel
    {
        public List<Ispit> Ispiti { get; set; } = null!;
        public Dictionary<int, string?> Rezultati { get; set; } = null!;
    }
}
