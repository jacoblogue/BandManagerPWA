using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils.Models;
using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.Services.Services
{
    public class EventService : IEventService
    {
        private ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync() ?? [];
        }

        public async Task<List<Event>> GetEventsByUserIdAsync(Guid id)
        {
            return await _context.Events.Where(e => e.Users.Any(u => u.Id == id)).ToListAsync() ?? [];
        }
    }
}
