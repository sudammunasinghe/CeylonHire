using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        public NotificationService(INotificationRepository notificationRepository, ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> GetUnreadNotificationCountAsync()
        {
            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            return await _notificationRepository.GetUnreadNotificationCountAsync(loggedUser);
        }

        public async Task MarkNotificationAsReadAsync(int id)
        {
            var notification =
                await _notificationRepository.GetNotificationRecipientByNotificationIdAsync(id);

            if (notification == null)
                throw new BadRequestException("Invalid notification id.");

            if (notification.IsRead)
                throw new ConflictException("Already marked as read.");

            notification.IsRead = true;
            notification.LastModifiedDateTime = DateTime.UtcNow;
            var updatedNotification = new List<NotificationRecipient>
            {
                notification
            };

            await _notificationRepository.MarkNotificationsAsReadAsync(updatedNotification);
        }

        public async Task MarkAllNotificationsAsRead()
        {
            var loggedUser = _currentUserService?.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var allNotifications =
                await _notificationRepository.GetAllUnReadNotificationsByUserIdAsync(loggedUser);

            if (!allNotifications.Any())
                throw new ConflictException("No any unread notification.");

            var updatedNotifications = allNotifications
                .Select(x => new NotificationRecipient
                {
                    IsRead = true,
                    LastModifiedDateTime = DateTime.UtcNow,
                }).ToList();

            await _notificationRepository.MarkNotificationsAsReadAsync(updatedNotifications);
        }
    }
}
