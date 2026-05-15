using CeylonHire.Application.DTOs.Subscription;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System.ComponentModel.Design;

namespace CeylonHire.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_JobseekerByUserId;
        private readonly string _Select_CompanyByCompanyId;
        private readonly string _Select_CompanySubscription;
        private readonly string _Insert_CompanySubscription;
        private readonly string _Update_CompanySubscription;
        private readonly string _Select_SubscribersByCompanyId;

        public SubscriptionRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobseekerByUserId = _queryLoader.Load("Subscription", "Select_JobseekerByUserId.sql");
            _Select_CompanyByCompanyId = _queryLoader.Load("Subscription", "Select_CompanyByCompanyId.sql");
            _Select_CompanySubscription = _queryLoader.Load("Subscription", "Select_CompanySubscription.sql");
            _Insert_CompanySubscription = _queryLoader.Load("Subscription", "Insert_CompanySubscription.sql");
            _Update_CompanySubscription = _queryLoader.Load("Subscription", "Update_CompanySubscription.sql");
            _Select_SubscribersByCompanyId = _queryLoader.Load("Subscription", "Select_SubscribersByCompanyId.sql");
        }

        public async Task<JobSeekerProfile?> GetJobSeekerByUserIdAsync(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<JobSeekerProfile>(
                _Select_JobseekerByUserId,
                new { UserId =  userId }
            );
        }

        public async Task<CompanyProfile?> GetCompanyByCompanyIdAsync(int companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyByCompanyId,
                new { CompanyId = companyId }
            );
        }

        public async Task<CompanySubscription?> GetCompanySubscriptonAsync(int jobseekerId, int companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanySubscription>(
                _Select_CompanySubscription,
                new
                {
                    JobSeekerId = jobseekerId,
                    CompanyId = companyId
                }
            );
        }

        public async Task SubscribeCompanyAsync(CompanySubscription subscription)
        {
            using var db = _connectionFactory.CreateConnection();
            if(subscription.Id > 0)
            {
                await db.ExecuteAsync(
                    _Update_CompanySubscription,
                    subscription
                );
                return;
            }
            await db.ExecuteAsync(
                _Insert_CompanySubscription,
                new
                {
                    JobseekerId = subscription?.JobseekerId,
                    CompanyId = subscription?.CompanyId
                }
            );
        }

        public async Task UnsubscribeCompanyAsync(CompanySubscription subscription)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_CompanySubscription,
                subscription
            );
        }

        public async Task<List<int>> GetSubscribedUsersByCompanyIdAsync(int companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            var subscribers = await db.QueryAsync<int>(
                _Select_SubscribersByCompanyId,
                new { CompanyId =  companyId }
            );
            return subscribers.ToList();
        }
    }
}
