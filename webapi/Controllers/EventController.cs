using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet, Authorize(Policy = "read:events")]
        public async Task<IActionResult> GetEvents()
        {

            var testEvents = new List<Event>
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 1",
                    Description = "Test Event 1 Description",
                    Location = "Test Event 1 Location",
                    Date = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 2",
                    Description = "Test Event 2 Description",
                    Location = "Test Event 2 Location",
                    Date = DateTime.Now.AddDays(2)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 3",
                    Description = "Test Event 3 Description",
                    Location = "Test Event 3 Location",
                    Date = DateTime.Now.AddDays(30)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 4",
                    Description = "Test Event 4 Description",
                    Location = "Test Event 4 Location",
                    Date = DateTime.Now.AddDays(40)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 5",
                    Description = "Test Event 5 Description",
                    Location = "Test Event 5 Location",
                    Date = DateTime.Now.AddDays(50)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 6",
                    Description = "Test Event 6 Description",
                    Location = "Test Event 6 Location",
                    Date = DateTime.Now.AddDays(60)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 7",
                    Description = "Test Event 7 Description",
                    Location = "Test Event 7 Location",
                    Date = DateTime.Now.AddDays(7)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 8",
                    Description = "Test Event 8 Description",
                    Location = "Test Event 8 Location",
                    Date = DateTime.Now.AddDays(8)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 9",
                    Description = "Test Event 9 Description",
                    Location = "Test Event 9 Location",
                    Date = DateTime.Now.AddDays(9)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 10",
                    Description = "Test",
                    Location = "Test Event 10 Location",
                    Date = DateTime.Now.AddDays(50)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 11",
                    Description = "Test Event 11 Description",
                    Location = "Test Event 11 Location",
                    Date = DateTime.Now.AddDays(11)
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Event 12",
                    Description = "Test Event 12 Description",
                    Location = "Test Event 12 Location",
                    Date = DateTime.Now.AddDays(100)
                },
            };

            var events = await _context.Events.ToListAsync();

            return Ok(events);
        }

        [HttpPost, Authorize(Policy = "create:events")]
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

            var message = new EventMessage
            {
                MessageType = MessageType.EventAdded,
                Event = newEvent
            };

            await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Policy = "delete:events")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);

            if (eventToDelete == null)
            {
                return NotFound();
            }

            _context.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            var message = new EventMessage
            {
                MessageType = MessageType.EventDeleted,
                EventId = id
            };
            await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);
            return Ok();
        }
    }
}
