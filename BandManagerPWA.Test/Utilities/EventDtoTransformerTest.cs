using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.DtoTransformers;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Test.Utilities
{
    [TestClass]
    public class EventDtoTransformerTest
    {
        [TestMethod]
        public void TransformToDtoTest()
        {
            // Arrange
            var @event = new Event
            {
                Date = new DateTimeOffset(2018, 1, 1, 0, 0, 0, TimeSpan.Zero),
                Description = "Test Description",
                Id = Guid.NewGuid(),
                Location = "Test Location",
                Title = "Test Title"
            };

            // Act
            var result = EventDtoTransformer.TransformToDto(@event);

            // Assert
            Assert.AreEqual(@event.Date, result.Date);
            Assert.AreEqual(@event.Description, result.Description);
            Assert.AreEqual(@event.Id, result.Id);
            Assert.AreEqual(@event.Location, result.Location);
            Assert.AreEqual(@event.Title, result.Title);
        }

        [TestMethod]
        public void TransformToEventTest()
        {
            // Arrange
            var eventDto = new EventDTO
            {
                Date = new DateTime(2018, 1, 1),
                Description = "Test Description",
                Id = Guid.NewGuid(),
                Location = "Test Location",
                Title = "Test Title"
            };

            var expectedUTCDate = TimeZoneInfo.ConvertTimeToUtc(eventDto.Date);
            // Act
            var result = EventDtoTransformer.TransformToEvent(eventDto);

            // Assert
            Assert.AreEqual(expectedUTCDate, result.Date);
            Assert.AreEqual(eventDto.Description, result.Description);
            Assert.AreEqual(eventDto.Id, result.Id);
            Assert.AreEqual(eventDto.Location, result.Location);
            Assert.AreEqual(eventDto.Title, result.Title);
        }
    }
}
