using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class UserProfileViewModel
    {
        public string Korisnickoime { get; set; } = null!;

        public string Identifikator { get; set; } = null!;

        public string Uloga { get; set; } = null!;

        public List<Statistika> Statistike_tecaja { get; set; } = null!;
        public List<Statistika> Statistike_ispita { get; set; } = null!;
    }
}
