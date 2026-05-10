using CeylonHire.Application.DTOs.Notification;
using CeylonHire.Application.DTOs.PagedResult;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

namespace CeylonHire.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_NotificationCountByUserId;
        private readonly string _Select_NotificationByNotificationId;
        private readonly string _Update_NotificationRecipient;
        private readonly string _Select_UnReadNotificationsByUserId;
        private readonly string _Select_AllFiteredNotificationsByUserId;

        public NotificationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_NotificationCountByUserId = _queryLoader.Load("Notification", "Select_NotificationCountByUserId.sql");
            _Select_NotificationByNotificationId = _queryLoader.Load("Notification", "Select_NotificationByNotificationId.sql");
            _Update_NotificationRecipient = _queryLoader.Load("Notification", "Update_NotificationRecipient.sql");
            _Select_UnReadNotificationsByUserId = _queryLoader.Load("Notification", "Select_UnReadNotificationsByUserId.sql");
            _Select_AllFiteredNotificationsByUserId = _queryLoader.Load("Notification", "Select_AllFiteredNotificationsByUserId.sql");
        }

        public async Task<PagedResult<NotificationDto>> GetNotificationsAsync(int pageNumber, int pageSize, int? userId)
        {
            var offset = (pageNumber - 1) * pageSize;
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(
                _Select_AllFiteredNotificationsByUserId,
                new
                {
                    UserId = userId,
                    Offset = offset,
                    PageSize = pageSize
                }
            );

            var items = (await multi.ReadAsync<NotificationDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<NotificationDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            };
        }
        public async Task<int> GetUnreadNotificationCountAsync(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                _Select_NotificationCountByUserId,
                new { UserId = userId }
            );
        }

        public async Task<NotificationRecipient?> GetNotificationRecipientByNotificationIdAsync(int id, int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<NotificationRecipient>(
                _Select_NotificationByNotificationId,
                new 
                { 
                    NotificationId = id,
                    RecipientUserId = userId
                }
            );
        }

        public async Task MarkNotificationsAsReadAsync(List<NotificationRecipient> updatedRecipient)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                await db.ExecuteAsync(
                    _Update_NotificationRecipient,
                    updatedRecipient,
                    transaction
                );
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<IEnumerable<NotificationRecipient>> GetAllUnReadNotificationsByUserIdAsync(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<NotificationRecipient>(
                _Select_UnReadNotificationsByUserId,
                new { RecipientUserId = userId }
            );
        }

        public async Task RemoveNotificationAsync(NotificationRecipient updatedRecipient)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_NotificationRecipient,
                updatedRecipient
            );
        }

    }
}
