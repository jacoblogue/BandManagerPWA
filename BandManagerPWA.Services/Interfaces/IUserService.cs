using BandManagerPWA.DataAccess.Models;

namespace BandManagerPWA.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserByEmailAsync(string? userEmail);
    }
}