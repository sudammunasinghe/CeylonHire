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
        private readonly string _Select_AllNotificationsByUserId;

        public NotificationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_NotificationCountByUserId = _queryLoader.Load("Notification", "Select_NotificationCountByUserId.sql");
            _Select_NotificationByNotificationId = _queryLoader.Load("Notification", "Select_NotificationByNotificationId.sql");
            _Update_NotificationRecipient = _queryLoader.Load("Notification", "Update_NotificationRecipient.sql");
            _Select_AllNotificationsByUserId = _queryLoader.Load("Notification", "Select_AllNotificationsByUserId.sql");
        }

        public async Task<int> GetUnreadNotificationCountAsync(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                _Select_NotificationCountByUserId,
                new { UserId = userId }
            );
        }

        public async Task<NotificationRecipient?> GetNotificationRecipientByNotificationIdAsync(int id)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<NotificationRecipient>(
                _Select_NotificationByNotificationId,
                new { NotificationId = id }
            );
        }

        public async Task MarkNotificationsAsReadAsync(List<NotificationRecipient> updatedRecipient)
        {
            using var db = _connectionFactory.CreateConnection();
            using var transaction = db.BeginTransaction();
            try
            {
                await db.ExecuteAsync(
                    _Update_NotificationRecipient,
                    updatedRecipient
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
                _Select_AllNotificationsByUserId,
                new { RecipientUserId = userId }
            );
        }

    }
}
