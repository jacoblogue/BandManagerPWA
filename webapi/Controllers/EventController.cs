using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using webapi.Models;
using BandManagerPWA.Utils.Models;
using webapi.utilities;
using BandManagerPWA.Utils;

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
        public EventController(IHubContext<EventHub> hubContext, IUserService userService, IEventService eventService)
        {
            _hubContext = hubContext;
            _userService = userService;
            _eventService = eventService;
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


                if (readAll)
                {
                    List<Event> events = await _eventService.GetAllEventsAsync();
                    // Convert to DTO
                    List<EventDTO> eventDTOs = EventDtoTransformer.TransformToDtoList(events);
                    return Ok(eventDTOs);
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
                    List<Event> events = await _eventService.GetEventsByUserIdAsync(user.Id);
                    // Convert to DTO
                    List<EventDTO> eventDTOs = EventDtoTransformer.TransformToDtoList(events);
                 
                    return Ok(eventDTOs);
                }
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                var returnEventDto = EventDtoTransformer.TransformToDto(newEvent);
                return Ok(returnEventDto);
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
            var eventToDelete = await _eventService.DeleteEventAsync(id);

            if (eventToDelete == null)
            {
                return NotFound();
            }

            var message = new EventMessage
            {
                MessageType = MessageType.EventDeleted,
                EventId = id
            };
            await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);

            var returnEventDto = EventDtoTransformer.TransformToDto(eventToDelete);
            Log.Information("Event deleted: {@Event}", eventToDelete);
            return Ok(returnEventDto);
        }

        [HttpPut, Authorize(Policy = "update:events")]
        public async Task<IActionResult> UpdateEvent(EventDTO eventToUpdateDTO)
        {
            try
            {
                Log.Information("UpdateEvent endpoint was hit.");

                bool writeAll = User.HasClaim("permissions", "write:all");
                if (writeAll)
                {
                    var eventToUpdate = await _eventService.GetEventByIdAsync(eventToUpdateDTO.Id);

                    if (eventToUpdate == null)
                    {
                        Log.Warning("Event not found");
                        return NotFound("Event not found");
                    }

                    var updatedEvent = await _eventService.UpdateEventAsync(eventToUpdateDTO);

                    var message = new EventMessage
                    {
                        MessageType = MessageType.EventUpdated,
                        Event = updatedEvent
                    };
                    await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);

                    var returnEventDto = EventDtoTransformer.TransformToDto(updatedEvent);
                    Log.Information("Event updated: {@Event}", updatedEvent);
                    return Ok(returnEventDto);
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

                    var eventToUpdate = await _eventService.GetEventByIdAsync(eventToUpdateDTO.Id);

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

                    var updatedEvent = await _eventService.UpdateEventAsync(eventToUpdateDTO);

                    var message = new EventMessage
                    {
                        MessageType = MessageType.EventUpdated,
                        Event = updatedEvent
                    };
                    await _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", message);

                    Log.Information("Event updated: {@Event}", updatedEvent);
                    EventDTO updatedEventDTO = new EventDTO
                    {
                        Id = updatedEvent.Id,
                        Title = updatedEvent.Title,
                        Description = updatedEvent.Description,
                        Location = updatedEvent.Location,
                        Date = updatedEvent.Date.UtcDateTime
                    };
                    return Ok(updatedEventDTO);
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
