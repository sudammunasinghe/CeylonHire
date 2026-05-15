namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IRealtimeNotificationService
    {
        Task SendRealTimeNotificationAsync(string title, string message, int notificationTypeId, List<int> recipientUsers);
    }
}
