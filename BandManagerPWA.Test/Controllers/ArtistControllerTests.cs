using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Security.Claims;
using webapi.Controllers;

namespace BandManagerPWA.Test.Controllers
{
    [TestClass]
    public class ArtistControllerTests
    {
        private ApplicationDbContext _context;
        private ArtistController _controller;
        private IArtistService _artistService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BandManagerPWA")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Artists.Add(new Artist { Id = Guid.NewGuid(), Name = "Test Artist" });
            _context.SaveChanges();

            _artistService = Substitute.For<IArtistService>();

            _controller = new ArtistController(_artistService);

            // Add User to ControllerContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser")
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }

        [TestMethod]
        public async Task GetArtists_CallsGetAllArtists()
        {
            // Act
            var result = await _controller.GetArtists();

            // Assert
            await _artistService.Received().GetAllArtistsAsync();
        }
    }
}
