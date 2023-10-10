using Microsoft.AspNetCore.SignalR;

namespace webapi.utilities
{
    public class EventHub : Hub
    {
        public async Task SendEventUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveEventUpdate", message);
        }
    }
}
