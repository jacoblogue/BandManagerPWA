using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webapi.Controllers;
using webapi.utilities;
using NSubstitute;
using Microsoft.AspNetCore.SignalR;
using BandManagerPWA.Services.Interfaces;

namespace BandManagerPWA.Test.Controllers
{
    [TestClass]
    public class EventControllerTests
    {
        private ApplicationDbContext _context;
        private EventController _controller;
        private IEventService _eventService;
        private IUserService _userService;

        [TestInitialize]
        public void Initialize()
        {
            // add application db context using inmemory sql server db
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BandManagerPWA")
                .Options;
            _context = new ApplicationDbContext(options);
            var hubContext = Substitute.For<IHubContext<EventHub>>();
            var clients = Substitute.For<IHubClients>();
            var allClients = Substitute.For<IClientProxy>();

            hubContext.Clients.Returns(clients);
            clients.All.Returns(allClients);

            _eventService = Substitute.For<IEventService>();
            _eventService.GetAllEventsAsync().Returns(new List<Event>());
            _eventService.GetEventsByUserIdAsync(Arg.Any<Guid>()).Returns(new List<Event>());

            _userService = Substitute.For<IUserService>();
            _userService.GetUserByEmailAsync(Arg.Any<string>()).Returns(new User());

            _controller = new EventController(hubContext, _userService, _eventService, _context);
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "delete:events"),
                new("https://bandmanager.com/email", "test@test.com")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            // Act
            var result = await _controller.Delete(newEvent.Id);
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var dbEvent = await _context.Events.FindAsync(newEvent.Id);
            Assert.IsNull(dbEvent);
        }

        [TestMethod]
        public async Task GetEvents_GetsAllEvents()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:all"),
                new("https://bandmanager.com/email", "test@test.com")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            // Act
            var result = await _controller.GetEvents();
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Event>));
            await _eventService.Received().GetAllEventsAsync();
        }

        [TestMethod]
        public async Task GetEvents_GetsUserEvents()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com"
            };

            var httpContextUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:events"),
                new("https://bandmanager.com/email", "test@test.com")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = httpContextUser
                }
            };

            // Act
            var result = await _controller.GetEvents();
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Event>));
            var events = okResult.Value as List<Event>;
            await _eventService.Received().GetEventsByUserIdAsync(Arg.Any<Guid>());
        }

        [TestMethod]
        public async Task UpdateEvent_WriteAll_UpdatesEvent()
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "write:all"),
                new("https://bandmanager.com/email", "")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var updatedEvent = new webapi.Models.EventDTO
            {
                Id = newEvent.Id,
                Title = "Updated Test Event",
                Date = DateTime.Now.AddDays(1),
                Location = "Updated Test Location",
                Description = "Updated Test Description"
            };

            // Act
            var result = await _controller.UpdateEvent(updatedEvent);
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Event));
            var dbEvent = okResult.Value as Event;
            Assert.AreEqual(updatedEvent.Id, dbEvent.Id);
            Assert.AreEqual(updatedEvent.Title, dbEvent.Title);
            Assert.AreEqual(updatedEvent.Date, dbEvent.Date);
            Assert.AreEqual(updatedEvent.Location, dbEvent.Location);
            Assert.AreEqual(updatedEvent.Description, dbEvent.Description);
        }

        [TestMethod]
        public async Task UpdateEvent_UpdatesEventWithUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com"
            };

            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Location = "Test Location",
                Description = "Test Description",
                Users = [user]
            };
            await _context.AddAsync(newEvent);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var httpContextUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "update:events"),
                new("https://bandmanager.com/email", "test@test.com")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = httpContextUser
                }
            };

            var updatedEvent = new webapi.Models.EventDTO
            {
                Id = newEvent.Id,
                Title = "Updated Test Event",
                Date = DateTime.Now.AddDays(1),
                Location = "Updated Test Location",
                Description = "Updated Test Description"
            };

            // Act
            var result = await _controller.UpdateEvent(updatedEvent);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Event));
            var dbEvent = okResult.Value as Event;
            Assert.AreEqual(updatedEvent.Id, dbEvent.Id);
            Assert.AreEqual(updatedEvent.Title, dbEvent.Title);
            Assert.AreEqual(updatedEvent.Date, dbEvent.Date);
            Assert.AreEqual(updatedEvent.Location, dbEvent.Location);
            Assert.AreEqual(updatedEvent.Description, dbEvent.Description);
            Assert.AreEqual(1, dbEvent.Users.Count);
            Assert.AreEqual(user.Id, dbEvent.Users[0].Id);
            Assert.AreEqual(user.Email, dbEvent.Users[0].Email);
        }
    }
}