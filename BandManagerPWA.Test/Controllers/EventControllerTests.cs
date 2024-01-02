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
using BandManagerPWA.Utils.Models;

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

            // Mock event service
            _eventService = Substitute.For<IEventService>();
            _eventService.GetAllEventsAsync().Returns(new List<Event>());
            _eventService.GetEventsByUserIdAsync(Arg.Any<Guid>()).Returns(new List<Event>());
            _eventService.CreateEventAsync(Arg.Any<EventDTO>()).Returns(new Event());
            _eventService.UpdateEventAsync(Arg.Any<EventDTO>()).Returns(new Event());
            _eventService.GetEventByIdAsync(Arg.Any<Guid>()).Returns(new Event());

            // Mock user service
            _userService = Substitute.For<IUserService>();
            _userService.GetUserByEmailAsync(Arg.Any<string>()).Returns(new User());

            _controller = new EventController(hubContext, _userService, _eventService, _context);
        }

        [TestMethod]
        public async Task CreateEvent_CallsCreateEventAsync()
        {
            // Arrange
            var newEvent = new EventDTO
            {
                Title = "New Event",
                Description = "New Event Description",
                Location = "New Event Location",
                Date = DateTime.Now
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "create:all"),
                new("https://bandmanager.com/email", "")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
            // Act
            var result = await _controller.CreateEvent(newEvent);
            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(EventDTO));
            await _eventService.Received().CreateEventAsync(Arg.Any<EventDTO>());
        }

        [TestMethod]
        public async Task Delete_CallsDeleteEventAsync()
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

            _eventService.DeleteEventAsync(Arg.Any<Guid>()).Returns(newEvent);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "delete:all"),
                new("https://bandmanager.com/email", "")
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
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(EventDTO));
            await _eventService.Received().DeleteEventAsync(Arg.Any<Guid>());
        }

        [TestMethod]
        public async Task GetEvents_ReadAll_CallsGetAllEventsAsync()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:all"),
                new("https://bandmanager.com/email", "")
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
            Assert.IsInstanceOfType(okResult.Value, typeof(List<EventDTO>));
            await _eventService.Received().GetAllEventsAsync();
        }

        [TestMethod]
        public async Task GetEvents_CallsGetEventsByUserIdAsync()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:events"),
                new("https://bandmanager.com/email", "")
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
            Assert.IsInstanceOfType(okResult.Value, typeof(List<EventDTO>));
            await _eventService.Received().GetEventsByUserIdAsync(Arg.Any<Guid>());
        }

        [TestMethod]
        public async Task UpdateEvent_WriteAll_CallsUpdateEvent()
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

            var updatedEvent = new EventDTO
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
            Assert.IsInstanceOfType(okResult.Value, typeof(EventDTO));
            await _eventService.Received().UpdateEventAsync(Arg.Any<EventDTO>());
        }

        [TestMethod]
        public async Task UpdateEvent_UserHasPermission_CallsUpdateEvent()
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

            // getEventbyIdAsync returns event with user in users list
            _userService.GetUserByEmailAsync(Arg.Any<string>()).Returns(user);
            _eventService.GetEventByIdAsync(Arg.Any<Guid>()).Returns(newEvent);

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

            var updatedEvent = new EventDTO
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
            Assert.IsInstanceOfType(okResult.Value, typeof(EventDTO));
            await _eventService.Received().UpdateEventAsync(Arg.Any<EventDTO>());
        }
    }
}