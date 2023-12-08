using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webapi.Controllers;
using webapi.Models;

namespace BandManagerPWA.Test.Controllers
{
    [TestClass]
    public class GroupControllerTests
    {
        private ApplicationDbContext _context;
        private GroupController _controller;


        [TestInitialize]
        public void Initialize()
        {
            // add application db context using inmemory sql server db
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BandManagerPWA")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new GroupController(_context);

            // add controllerContext with user claims

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task GetGroups_GetsAllGroups()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:all"),
                new("https://bandmanager.com/email", "test@test.com")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };


            // Act
            var result = await _controller.GetGroups();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Group>));
            var groups = okResult.Value as List<Group>;
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual(newGroup.Id, groups[0].Id);
            Assert.AreEqual(newGroup.Name, groups[0].Name);
            Assert.AreEqual(newGroup.Description, groups[0].Description);
        }

        [TestMethod]
        public async Task GetGroups_GetsGroupsForUser()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description",
                Users = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "test@test.com"
                    }
                }
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("permissions", "read:groups"),
                new("https://bandmanager.com/email", "test@test.com")
             }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };


            // Act
            var result = await _controller.GetGroups();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Group>));
            var groups = okResult.Value as List<Group>;
            Assert.AreEqual(1, groups.Count);
            Assert.AreEqual(newGroup.Id, groups[0].Id);
            Assert.AreEqual(newGroup.Name, groups[0].Name);
            Assert.AreEqual(newGroup.Description, groups[0].Description);
        }

        [TestMethod]
        public async Task GetGroup_GetsGroup()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetGroup(newGroup.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Group));
            var group = okResult.Value as Group;
            Assert.AreEqual(newGroup.Id, group.Id);
            Assert.AreEqual(newGroup.Name, group.Name);
            Assert.AreEqual(newGroup.Description, group.Description);
        }

        [TestMethod]
        public async Task GetGroup_ReturnsNotFound()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetGroup(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateGroup_CreatesGroup()
        {
            // Arrange
            var newGroup = new GroupDTO
            {
                Name = "Test Group",
                Description = "Test Description"
            };

            // Act
            var result = await _controller.CreateGroup(newGroup);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(Group));
            var createdGroup = okResult.Value as Group;
            Assert.AreEqual(newGroup.Name, createdGroup.Name);
            Assert.AreEqual(newGroup.Description, createdGroup.Description);
            var dbGroup = await _context.Groups.FindAsync(createdGroup.Id);
            Assert.IsNotNull(dbGroup);
        }

        [TestMethod]
        public async Task Delete_DeletesGroup()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteGroup(newGroup.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var dbGroup = await _context.Groups.FindAsync(newGroup.Id);
            Assert.IsNull(dbGroup);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteGroup(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateGroupInfo_UpdatesGroup()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var updatedGroup = new GroupDTO
            {
                Name = "Updated Group",
                Description = "Updated Description"
            };

            // Act
            var result = await _controller.UpdateGroupInfo(newGroup.Id, updatedGroup);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObjectResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(Group));
            var dbGroup = okObjectResult.Value as Group;
            Assert.AreEqual(newGroup.Id, dbGroup.Id);
            Assert.AreEqual(updatedGroup.Name, dbGroup.Name);
            Assert.AreEqual(updatedGroup.Description, dbGroup.Description);
        }

        [TestMethod]
        public async Task UpdateGroupInfo_ReturnsNotFound()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var updatedGroup = new GroupDTO
            {
                Name = "Updated Group",
                Description = "Updated Description"
            };

            // Act
            var result = await _controller.UpdateGroupInfo(Guid.NewGuid(), updatedGroup);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task AddMember_AddsMember()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var userEmail = "test@test.com";

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail
            };
            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.AddMember(newGroup.Id, userEmail);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var dbGroup = await _context.Groups.FindAsync(newGroup.Id);
            Assert.AreEqual(1, dbGroup.Users.Count);
            Assert.AreEqual(existingUser.Id, dbGroup.Users[0].Id);
            Assert.AreEqual(existingUser.Email, dbGroup.Users[0].Email);
        }

        [TestMethod]
        public async Task RemoveMember_RemovesMember()
        {
            // Arrange
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Test Group",
                Description = "Test Description"
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            var userEmail = "test@test.com";

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = userEmail
            };
            await _context.Users.AddAsync(existingUser);
            newGroup.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.RemoveMember(newGroup.Id, existingUser.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var dbGroup = await _context.Groups.FindAsync(newGroup.Id);
            Assert.AreEqual(0, dbGroup.Users.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}