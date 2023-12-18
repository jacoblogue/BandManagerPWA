using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using webapi.Models;
using BandManagerPWA.Utils.Models;
using webapi.utilities;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IHubContext<EventHub> _hubContext;
        private readonly string _emailClaimType = "https://bandmanager.com/email";
        private IEventService _eventService;
        private IUserService _userService;
        private ApplicationDbContext _context;
        public EventController(IHubContext<EventHub> hubContext, IUserService userService, IEventService eventService, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _userService = userService;
            _eventService = eventService;
            _context = context;
        }

        [HttpGet, Authorize(Policy = "read:events")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
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
                    events = await _eventService.GetAllEventsAsync();
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
                    var user = await _userService.GetUserByEmailAsync(userEmail);

                    if (user == null)
                    {
                        Log.Warning("User not found");
                        return BadRequest("User not found");
                    }

                    // TODO: Pagination?
                    events = await _eventService.GetEventsByUserIdAsync(user.Id);
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
            try
            {
                Event newEvent = await _eventService.CreateEventAsync(incomingEvent);

                var message = new EventMessage
                {
                    MessageType = MessageType.EventAdded,
                    Event = newEvent
                };

                await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);

                Log.Information("Event created: {@Event}", newEvent);
                return Ok(newEvent);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Problem(ex.Message);
            }
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

                bool writeAll = User.HasClaim("permissions", "write:all");
                if (writeAll)
                {
                    var eventToUpdate = _context.Events.FirstOrDefault(e => e.Id == updatedEvent.Id);

                    if (eventToUpdate == null)
                    {
                        Log.Warning("Event not found");
                        return NotFound("Event not found");
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

                    var eventToUpdate = await _context.Events.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == updatedEvent.Id);

                    if (eventToUpdate == null)
                    {
                        Log.Warning("Event not found");
                        return NotFound("Event not found");
                    }

                    if (!eventToUpdate.Users.Any(u => u.Id == user.Id))
                    {
                        Log.Warning("User does not have permission to update this event");
                        return BadRequest("User does not have permission to update this event");
                    }

                    eventToUpdate.Title = updatedEvent.Title;
                    eventToUpdate.Description = updatedEvent.Description;
                    eventToUpdate.Location = updatedEvent.Location;
                    eventToUpdate.Date = updatedEvent.Date.ToUniversalTime();

                    await _context.SaveChangesAsync();
                    return Ok(eventToUpdate);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
