using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class ModeratorTecajiViewModel
    {
        public List<Tecaj> Tecaji { get; set; } = null!;
    }

    public class TecajPoredakViewModel
    {
        public int Id { get; set; }
        public double Pozicija { get; set; }
    }
}
