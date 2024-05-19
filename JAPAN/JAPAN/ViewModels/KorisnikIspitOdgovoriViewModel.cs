namespace JAPAN.ViewModels
{
    public class KorisnikIspitOdgovoriViewModel
    {
        public int UserId { get; set; }
        public int IspitId { get; set; }
        public Dictionary<int, int> PitanjeOdgovori { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, string> PitanjeOtvoreniOdgovori { get; set; } = new Dictionary<int, string>();
    }
}
