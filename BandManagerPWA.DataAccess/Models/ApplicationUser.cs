using Microsoft.AspNetCore.Identity;

namespace BandManagerPWA.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
