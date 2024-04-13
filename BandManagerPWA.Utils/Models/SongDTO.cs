namespace BandManagerPWA.Utils.Models
{
    public class SongDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string? Key { get; set; }
    }
}
