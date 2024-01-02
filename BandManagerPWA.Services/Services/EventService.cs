using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils;
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

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync() ?? [];
        }

        public async Task<List<Event>> GetEventsByUserIdAsync(Guid id)
        {
            return await _context.Events.Where(e => e.Users.Any(u => u.Id == id)).ToListAsync() ?? [];
        }

        public async Task<Event> CreateEventAsync(EventDTO newEventDTO)
        {
            var newEvent = EventDtoTransformer.TransformToEvent(newEventDTO);

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event> UpdateEventAsync(EventDTO updatedEventDTO)
        {
            var existingEvent = await _context.Events.FindAsync(updatedEventDTO.Id);

            if (existingEvent is null)
            {
                throw new Exception("Event not found");
            }

            existingEvent.Title = updatedEventDTO.Title;
            existingEvent.Description = updatedEventDTO.Description;
            existingEvent.Location = updatedEventDTO.Location;
            existingEvent.Date = updatedEventDTO.Date.ToUniversalTime();

            await _context.SaveChangesAsync();
            return existingEvent;
        }

        public async Task<Event?> DeleteEventAsync(Guid id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete != null)
            {
                _context.Remove(eventToDelete);
                await _context.SaveChangesAsync();
                return eventToDelete;
            }

            return null;
        }
    }
}
