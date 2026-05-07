using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface INotificationRepository
    {
        Task<int> GetUnreadNotificationCountAsync(int? userId);
        Task<NotificationRecipient?> GetNotificationRecipientByNotificationIdAsync(int id);
        Task MarkNotificationsAsReadAsync(List<NotificationRecipient> updatedRecipient);
        Task<IEnumerable<NotificationRecipient>> GetAllUnReadNotificationsByUserIdAsync(int? userId);
    }
}
