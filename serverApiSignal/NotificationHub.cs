using Microsoft.AspNetCore.SignalR;

namespace serverApiSignal
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(NotificationDto data)
        {
            await Clients.All.SendAsync("ReceiveNotification", data);
        }
    }
}
