namespace BandManagerPWA.DataAccess.Models
{
    public class Song : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public string? Key { get; set; }
    }
}
