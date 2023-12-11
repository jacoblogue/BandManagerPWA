using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        private readonly string _emailClaimType = "https://bandmanager.com/email";
        public EventController(ApplicationDbContext context, IHubContext<EventHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet, Authorize(Policy = "read:events")]
        public async Task<IActionResult> GetEvents()
        {
            try
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
                Log.Information("GetEvents endpoint hit");

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                bool readAll = User.HasClaim("permissions", "read:all");

                List<Event> events = [];

                if (readAll)
                {
                    events = await _context.Events.ToListAsync();
                    return Ok(events);
                }
                else
                {
                    if (!User.HasClaim(c => c.Type == _emailClaimType))
                    {
                        Log.Warning("User email claim not found");
                        return BadRequest("User email claim not found");
                    }

                    // get user's email and only get their events
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == _emailClaimType)?.Value;
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (user == null)
                    {
                        Log.Warning("User not found");
                        return BadRequest("User not found");
                    }

                    // TODO: Pagination?
                    events = await _context.Events.Where(e => e.Users.Any(u => u.Id == user.Id)).ToListAsync();
                }

                return Ok(events);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Problem(ex.Message);
            }
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

            Log.Information("Event created: {@Event}", newEvent);
            return Ok(newEvent);
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

            Log.Information("Event deleted: {@Event}", eventToDelete);
            return Ok();
        }

        [HttpPut, Authorize(Policy = "update:events")]
        public async Task<IActionResult> UpdateEvent(EventDTO updatedEvent)
        {
            try
            {
                Log.Information("UpdateEvent endpoint was hit.");
                var eventToUpdate = _context.Events.FirstOrDefault(e => e.Id == updatedEvent.Id);

                if (eventToUpdate == null)
                {
                    Log.Warning("Event not found");
                    return NotFound();
                }

                eventToUpdate.Title = updatedEvent.Title;
                eventToUpdate.Description = updatedEvent.Description;
                eventToUpdate.Location = updatedEvent.Location;
                eventToUpdate.Date = updatedEvent.Date.ToUniversalTime();

                await _context.SaveChangesAsync();

                var message = new EventMessage
                {
                    MessageType = MessageType.EventUpdated,
                    Event = eventToUpdate
                };
                await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);

                Log.Information("Event updated: {@Event}", eventToUpdate);
                return Ok(eventToUpdate);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
