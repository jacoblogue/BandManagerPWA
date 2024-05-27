using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Services;
using BandManagerPWA.Utils.Models;
using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.Services.ServicesTests
{
    [TestClass()]
    public class ArtistServiceTests
    {
        private ArtistService _artistService;
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BandManagerPWA")
                .Options;
            _context = new ApplicationDbContext(options);
            _artistService = new ArtistService(_context);
        }

        [TestMethod()]
        public void GetArtistByIdAsyncTest()
        {
            // Arrange
            var artistId = Guid.NewGuid();
            _context.Add(new Artist { Id = artistId, Name = "John Doe" });


            // Act
            var artist = _artistService.GetArtistByIdAsync(artistId).Result;

            // Assert
            Assert.IsNotNull(artist);
            Assert.AreEqual(artistId, artist.Id);
        }

        [TestMethod()]
        public void GetArtistByNameAsyncTest()
        {
            // Arrange
            string artistName = "John Doe";
            _context.Add(new Artist { Name = artistName });
            _context.SaveChanges();

            // Act
            var artist = _artistService.GetArtistByNameAsync(artistName).Result;

            // Assert
            Assert.IsNotNull(artist);
            Assert.AreEqual(artistName, artist.Name);
        }

        [TestMethod()]
        public void GetAllArtistsAsyncTest()
        {
            // Arrange
            _context.Add(new Artist { Name = "John Doe" });
            _context.Add(new Artist { Name = "Jane Smith" });
            _context.SaveChanges();

            // Act
            var artists = _artistService.GetAllArtistsAsync().Result;

            // Assert
            Assert.IsNotNull(artists);
            Assert.IsTrue(artists.Count > 0);
        }

        [TestMethod()]
        public void CreateArtistAsyncTest()
        {
            // Arrange
            var newArtistDTO = new ArtistDTO
            {
                Name = "Jane Smith",
            };

            // Act
            var createdArtist = _artistService.CreateArtistAsync(newArtistDTO).Result;

            // Assert
            Assert.IsNotNull(createdArtist);
            Assert.AreEqual(newArtistDTO.Name, createdArtist.Name);
        }
    }
}