namespace JAPAN.ViewModels
{
    public class UrediIspitViewModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public string Opis { get; set; } = null!;
        public int TezinaId { get; set; }
        public List<int> PitanjaId { get; set; } = [];
    }
}
