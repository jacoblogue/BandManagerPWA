namespace BandManagerPWA.DataAccess.Models
{
    public class Artist : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
