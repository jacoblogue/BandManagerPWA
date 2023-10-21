using Microsoft.AspNetCore.SignalR;

namespace BandManagerPWA.Test
{
    public class FakeHubContext<THub> : IHubContext<THub> where THub : Hub
    {
        public IHubClients Clients { get; set; }  // Initialize these if your tests need to inspect their behavior
        public IGroupManager Groups { get; set; }  // Initialize these if your tests need to inspect their behavior

        public FakeHubContext()
        {
        }
    }
}
