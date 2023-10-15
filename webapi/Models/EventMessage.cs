using BandManagerPWA.DataAccess.Models;

namespace webapi.Models
{
    public class EventMessage
    {
        public string MessageType { get; set; }
        public Guid? EventId { get; set; }

        public Event? Event { get; set; }
    }
}
