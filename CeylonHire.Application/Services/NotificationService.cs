using CeylonHire.Application.DTOs.Notification;
using CeylonHire.Application.DTOs.PagedResult;
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
        private readonly IRealtimeNotificationService _realtimeNotificationService;
        public NotificationService(
            INotificationRepository notificationRepository,
            ICurrentUserService currentUserService,
            IRealtimeNotificationService realtimeNotificationService
            )
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
            _realtimeNotificationService = realtimeNotificationService;
        }

        public async Task SendNotificationAsync(
            string title,
            string message,
            int notificationTypeId,
            List<int> recipientUsers
        )
        {
            //Save notification to DB
            var loggedUser = _currentUserService.UserId;
            var notification = new Notification
            {
                Title = title,
                Message = message,
                SentUserId = loggedUser,
                NotificationTypeId = notificationTypeId
            };
            await _notificationRepository.SendNotificationAsync(notification, recipientUsers);

            //Realtime update using SignalR
            await _realtimeNotificationService.SendRealTimeNotificationAsync(
                title,
                message,
                notificationTypeId,
                recipientUsers
            );
        }

        public async Task<PagedResult<NotificationDto>> GetNotificationsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters.");

            var loggedUser = _currentUserService.UserId;
            return await _notificationRepository.GetNotificationsAsync(pageNumber, pageSize, loggedUser);
        }

        public async Task<int> GetUnreadNotificationCountAsync()
        {
            var loggedUser = _currentUserService.UserId;
            return await _notificationRepository.GetUnreadNotificationCountAsync(loggedUser);
        }

        public async Task MarkNotificationAsReadAsync(int id)
        {
            var loggedUser = _currentUserService.UserId;
            var notification =
                await _notificationRepository.GetNotificationRecipientByNotificationIdAsync(id, loggedUser);

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
            var loggedUser = _currentUserService.UserId;
            var allNotifications =
                await _notificationRepository.GetAllUnReadNotificationsByUserIdAsync(loggedUser);

            if (!allNotifications.Any())
                throw new ConflictException("No any unread notification.");

            var updatedNotifications = allNotifications
                .Select(x =>
                {
                    x.IsRead = true;
                    x.LastModifiedDateTime = DateTime.UtcNow;
                    return x;
                }).ToList();

            await _notificationRepository.MarkNotificationsAsReadAsync(updatedNotifications);
        }

        public async Task RemoveNotificationAsync(int id)
        {
            var loggedUser = _currentUserService.UserId;
            var notification =
                await _notificationRepository.GetNotificationRecipientByNotificationIdAsync(id, loggedUser);

            if (notification == null)
                throw new BadRequestException("Invalid notification id.");

            notification.IsActive = false;
            notification.LastModifiedDateTime = DateTime.UtcNow;
            await _notificationRepository.RemoveNotificationAsync(notification);
        }
    }
}
