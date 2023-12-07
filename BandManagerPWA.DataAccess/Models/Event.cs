namespace BandManagerPWA.DataAccess.Models
{
    public class Event : BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<User> Users { get; set; } = new List<User>();
    }
}
