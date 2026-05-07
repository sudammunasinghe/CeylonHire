using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_NotificationCountByUserId;
        
        public NotificationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_NotificationCountByUserId = _queryLoader.Load("Notification", "Select_NotificationCountByUserId.sql");
        }

        public async Task<int> GetUnreadNotificationCount(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                _Select_NotificationCountByUserId,
                new { UserId =  userId }
            );
        }
    }
}
