namespace BandManagerPWA.DataAccess.Models
{
    public class Group : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
