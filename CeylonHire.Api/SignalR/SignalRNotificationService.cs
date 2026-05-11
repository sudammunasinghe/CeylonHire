using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace CeylonHire.Api.SignalR
{
    public class SignalRNotificationService : IRealtimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        
        public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendRealTimeNotificationAsync(
            string title, 
            string message, 
            int notificationTypeId, 
            List<int> recipientUsers
            )
        {
            foreach (var user in recipientUsers)
            {
                await _hubContext.Clients
                    .User(user.ToString())
                    .SendAsync("ReceiveNotification", new
                    {
                        Title = title,
                        Message = message,
                        NotificationTypeId = notificationTypeId
                    });
            }
        }
    }
}
