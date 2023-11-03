namespace BandManagerPWA.DataAccess.Models
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public List<Group>? Groups { get; set; }
    }
}
