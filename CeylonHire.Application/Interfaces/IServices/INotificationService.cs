using CeylonHire.Application.DTOs.Notification;
using CeylonHire.Application.DTOs.PagedResult;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationDto>> GetNotificationsAsync(int pageNumber, int pageSize);
        Task<int> GetUnreadNotificationCountAsync();
        Task MarkNotificationAsReadAsync(int id);
        Task MarkAllNotificationsAsRead();
        Task RemoveNotificationAsync(int id);
    }
}
