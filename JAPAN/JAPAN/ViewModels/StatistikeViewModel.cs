using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class StatistikeViewModel
    {
        public List<TecajStatistike> TecajStatistike { get; set; } = null!;
        public List<IspitStatistike> IspitStatistike { get; set; } = null!;
    }

    public class TecajStatistike
    {
        public Tecaj Tecaj { get; set; } = null!;
        public List<Statistika> Statistike { get; set; } = null!;
    }

    public class IspitStatistike
    {
        public Ispit Ispit { get; set; } = null!;
        public List<Statistika> Statistike { get; set; } = null!;
    }
}
