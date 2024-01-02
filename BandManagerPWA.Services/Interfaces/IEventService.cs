using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Services.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Creates a new event based on the provided data transfer object.
        /// </summary>
        /// <param name="newEventDTO">The data transfer object containing event information.</param>
        /// <returns>The newly created event.</returns>
        Task<Event> CreateEventAsync(EventDTO newEventDTO);

        /// <summary>
        /// Deletes an event by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>The deleted event if found; otherwise, null.</returns>
        Task<Event?> DeleteEventAsync(Guid id);

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        Task<List<Event>> GetAllEventsAsync();

        /// <summary>
        /// Retrieves an event by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>The event if found; otherwise, null.</returns>
        Task<Event?> GetEventByIdAsync(Guid id);

        /// <summary>
        /// Retrieves events associated with a specific user.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A list of events linked to the specified user.</returns>
        Task<List<Event>> GetEventsByUserIdAsync(Guid id);
        
        /// <summary>
        /// Updates an existing event with new information.
        /// </summary>
        /// <param name="updatedEventDTO">The data transfer object containing the updated event information.</param>
        /// <returns>The updated event.</returns>
        /// <exception cref="Exception">Thrown when the event is not found.</exception>
        Task<Event> UpdateEventAsync(EventDTO updatedEventDTO);
    }
}