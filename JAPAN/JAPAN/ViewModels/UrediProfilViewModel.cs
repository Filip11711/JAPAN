namespace JAPAN.ViewModels
{
    public class UrediProfilViewModel
    {
        public int Id { get; set; }
        public string? Korisnickoime { get; set; }
        public string? Email { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public DateOnly? DatumRodenja { get; set; }
        public string? Preporuka { get; set; }
        public string? Uloga { get; set; }
    }
}
