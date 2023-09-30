using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var testEvents = new Event[]
            {
                new Event { Id = Guid.NewGuid(), Title = "My Event", Description = "My Description", Location = "My Location", Date = new System.DateTime(2020, 1, 1) },
                new Event { Id = Guid.NewGuid(), Title = "My Event 2", Description = "My Description 2", Location = "My Location 2", Date = new System.DateTime(2020, 1, 2) },
                new Event { Id = Guid.NewGuid(), Title = "My Event 3", Description = "My Description 3", Location = "My Location 3", Date = new System.DateTime(2020, 1, 3) },
                new Event { Id = Guid.NewGuid(), Title = "My Event 4", Description = "My Description 4", Location = "My Location 4", Date = new System.DateTime(2020, 1, 4) },
                new Event { Id = Guid.NewGuid(), Title = "My Event 5", Description = "My Description 5", Location = "My Location 5", Date = new System.DateTime(2020, 1, 5) },
            };

            return Ok(testEvents);
        }
    }
}
