namespace CeylonHire.Application.Interfaces.IServices
{
    public interface INotificationService
    {
        Task<int> GetUnreadNotificationCountAsync();
        Task MarkNotificationAsReadAsync(int id);
        Task MarkAllNotificationsAsRead();
    }
}
