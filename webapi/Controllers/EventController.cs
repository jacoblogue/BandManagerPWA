using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
        {
            var testEvents = new EventDTO[]
            {
                new EventDTO { Id = Guid.NewGuid(), Title = "My Event", Description = "My Description", Location = "My Location", Date = new DateTime(2020, 1, 1) },
                new EventDTO { Id = Guid.NewGuid(), Title = "My Event 2", Description = "My Description 2", Location = "My Location 2", Date = new DateTime(2020, 1, 2) },
                new EventDTO { Id = Guid.NewGuid(), Title = "My Event 3", Description = "My Description 3", Location = "My Location 3", Date = new DateTime(2020, 1, 3) },
                new EventDTO { Id = Guid.NewGuid(), Title = "My Event 4", Description = "My Description 4", Location = "My Location 4", Date = new DateTime(2020, 1, 4) },
                new EventDTO { Id = Guid.NewGuid(), Title = "My Event 5", Description = "My Description 5", Location = "My Location 5", Date = new DateTime(2020, 1, 5) },
            };

            var events = _context.Events.ToList();

            return Ok(events);
        }

        [HttpPost]
        public IActionResult Post([FromBody] EventDTO incomingEvent)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = incomingEvent.Title,
                Description = incomingEvent.Description,
                Location = incomingEvent.Location,
                Date = incomingEvent.Date
            };

            _context.Add(newEvent);
            _context.SaveChanges();

            return Ok(newEvent);
        }
    }
}
