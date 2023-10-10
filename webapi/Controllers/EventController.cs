using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.utilities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly IHubContext<EventHub> _hubContext;
        public EventController(ApplicationDbContext context, IHubContext<EventHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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

            var message = new
            {
                type = "eventAdded",
                eventId = newEvent.Id
            };

            await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);
            return Ok();
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

            var message = new
            {
                type = "eventDeleted",
                eventId = id
            };
            await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);
            return Ok();
        }
    }
}
