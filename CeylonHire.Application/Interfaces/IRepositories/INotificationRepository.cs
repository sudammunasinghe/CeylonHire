using CeylonHire.Application.DTOs.Notification;
using CeylonHire.Application.DTOs.PagedResult;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface INotificationRepository
    {
        Task SendNotificationAsync(Notification notification, List<int> recipientUsers);
        Task<PagedResult<NotificationDto>> GetNotificationsAsync(int pageNumber, int pageSize, int? userId);
        Task<int> GetUnreadNotificationCountAsync(int? userId);
        Task<NotificationRecipient?> GetNotificationRecipientByNotificationIdAsync(int id, int? userId);
        Task MarkNotificationsAsReadAsync(List<NotificationRecipient> updatedRecipient);
        Task<IEnumerable<NotificationRecipient>> GetAllUnReadNotificationsByUserIdAsync(int? userId);
        Task RemoveNotificationAsync(NotificationRecipient updatedRecipient);
    }
}
