using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Utils.DtoTransformers
{
    /// <summary>
    /// Class responsible for transforming Event objects to EventDTO objects and vice versa.
    /// </summary>
    public class EventDtoTransformer
    {
        /// <summary>
        /// Transforms an Event object to an EventDTO object.
        /// </summary>
        /// <param name="event">The Event object to transform.</param>
        /// <returns>The transformed EventDTO object.</returns>
        public static EventDTO TransformToDto(Event @event)
        {
            return new EventDTO
            {
                Id = @event.Id,
                Date = @event.Date.UtcDateTime,
                Description = @event.Description,
                Location = @event.Location,
                Title = @event.Title
            };
        }

        /// <summary>
        /// Transforms an EventDTO object to an Event object.
        /// </summary>
        /// <param name="eventDto">The EventDTO object to transform.</param>
        /// <returns>The transformed Event object.</returns>
        public static Event TransformToEvent(EventDTO eventDto)
        {
            return new Event
            {
                Id = eventDto.Id,
                Date = new DateTimeOffset(eventDto.Date).ToUniversalTime(),
                Description = eventDto.Description,
                Location = eventDto.Location,
                Title = eventDto.Title
            };
        }

        /// <summary>
        /// Transforms a list of Event objects to a list of EventDTO objects.
        /// </summary>
        /// <param name="events">The list of Event objects to transform.</param>
        /// <returns>The transformed list of EventDTO objects.</returns>
        public static List<EventDTO> TransformToDtoList(List<Event> events)
        {
            return events.Select(TransformToDto).ToList();
        }

        /// <summary>
        /// Transforms a list of EventDTO objects to a list of Event objects.
        /// </summary>
        /// <param name="eventDtos">The list of EventDTO objects to transform.</param>
        /// <returns>The transformed list of Event objects.</returns>
        public static List<Event> TransformToEventList(List<EventDTO> eventDtos)
        {
            return eventDtos.Select(TransformToEvent).ToList();
        }
    }
}
