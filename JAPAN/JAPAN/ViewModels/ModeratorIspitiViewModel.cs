using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class ModeratorIspitiViewModel
    {
        public List<Ispit> Ispiti { get; set; } = null!;
    }

    public class IspitPoredakViewModel
    {
        public int Id { get; set; }
        public double Pozicija { get; set; }
    }
}
