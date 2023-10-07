namespace BandManagerPWA.DataAccess.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
