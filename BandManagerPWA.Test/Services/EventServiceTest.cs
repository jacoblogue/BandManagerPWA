using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Services;
using BandManagerPWA.Utils.Models;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers;

namespace BandManagerPWA.Test.Services
{
    [TestClass]
    public class EventServiceTest
    {
        private ApplicationDbContext _context;
        private EventService _eventService;
        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EventServiceTestDB")
                .Options;

            _context = new ApplicationDbContext(options);
            _eventService = new EventService(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = new Guid(), Title = "Event 1" },
                new Event { Id = new Guid(), Title = "Event 2" },
                new Event { Id = new Guid(), Title = "Event 3" }
            };

            _context.Events.AddRange(events);
            _context.SaveChanges();

            // Act
            var result = await _eventService.GetAllEventsAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Event 1", result[0].Title);
            Assert.AreEqual("Event 2", result[1].Title);
            Assert.AreEqual("Event 3", result[2].Title);
        }

        [TestMethod]
        public async Task CreateEventAsync_CreatesNewEvent()
        {
            // Arrange
            var newEvent = new EventDTO
            {
                Title = "New Event",
                Description = "New Event Description",
                Location = "New Event Location",
                Date = new DateTime(2018, 1, 1)
            };

            // Act
            var result = await _eventService.CreateEventAsync(newEvent);

            // Assert
            Assert.AreEqual(newEvent.Title, result.Title);
            Assert.AreEqual(newEvent.Description, result.Description);
            Assert.AreEqual(newEvent.Location, result.Location);
            Assert.AreEqual(newEvent.Date, result.Date);
        }
    }
}
