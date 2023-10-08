using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();

            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO incomingEvent)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = incomingEvent.Title,
                Description = incomingEvent.Description,
                Location = incomingEvent.Location,
                Date = incomingEvent.Date.ToUniversalTime()
            };

            await _context.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            return Ok(newEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);

            if (eventToDelete == null)
            {
                return NotFound();
            }

            _context.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
