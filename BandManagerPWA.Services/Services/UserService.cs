using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BandManagerPWA.Services.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<User?> GetUserByEmailAsync(string? userEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    return null;
                }

                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    Log.Warning($"User with email {userEmail} not found");
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting user by email");
                throw;
            }
        }
    }
}
