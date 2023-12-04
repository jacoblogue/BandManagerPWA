using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers;
using webapi.utilities;

namespace BandManagerPWA.Test.Controllers
{
    [TestClass]
    public class EventControllerTests
    {
        private ApplicationDbContext _context;
        private EventController _controller;

        [TestInitialize]
        public void Initialize()
        {
            // add application db context using inmemory sql server db
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BandManagerPWA")
                .Options;
            _context = new ApplicationDbContext(options);
            var hubContext = new FakeHubContext<EventHub>();
            _controller = new EventController(_context, hubContext);
        }

        [TestMethod]
        public async Task Create_CreatesNewEvent()
        {
            // Arrange
            var newEvent = new webapi.Models.EventDTO
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Location = "Test Location",
                Description = "Test Description"
            };
            // Act
            var result = await _controller.CreateEvent(newEvent);
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Event));
            var createdEvent = okResult.Value as Event;
            Assert.AreEqual(newEvent.Title, createdEvent.Title);
            Assert.AreEqual(newEvent.Date, createdEvent.Date);
            Assert.AreEqual(newEvent.Location, createdEvent.Location);
            Assert.AreEqual(newEvent.Description, createdEvent.Description);
            var dbEvent = await _context.Events.FindAsync(createdEvent.Id);
            Assert.IsNotNull(dbEvent);
        }

        [TestMethod]
        public async Task Delete_DeletesEvent()
        {
            // Arrange
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Location = "Test Location",
                Description = "Test Description"
            };
            await _context.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            // Act
            var result = await _controller.Delete(newEvent.Id);
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var dbEvent = await _context.Events.FindAsync(newEvent.Id);
            Assert.IsNull(dbEvent);
        }

        [TestMethod]
        public async Task Get_GetsEvents()
        {
            // Arrange
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Location = "Test Location",
                Description = "Test Description"
            };
            await _context.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            // Act
            var result = await _controller.GetEvents();
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Event>));
            var events = okResult.Value as List<Event>;
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(newEvent.Id, events[0].Id);
            Assert.AreEqual(newEvent.Title, events[0].Title);
            Assert.AreEqual(newEvent.Date, events[0].Date);
            Assert.AreEqual(newEvent.Location, events[0].Location);
            Assert.AreEqual(newEvent.Description, events[0].Description);
        }
    }
}