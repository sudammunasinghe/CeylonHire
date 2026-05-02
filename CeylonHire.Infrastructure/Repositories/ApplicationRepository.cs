using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
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
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Select_JobByJobId;
        private readonly string _Select_JobApplication;
        private readonly string _Insert_JobApplication;
        
        public ApplicationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobByJobId = _queryLoader.Load("Application", "Select_JobByJobId.sql");
            _Select_JobApplication = _queryLoader.Load("Application", "Select_JobApplication.sql");
            _Insert_JobApplication = _queryLoader.Load("Application", "Insert_JobApplication.sql");
        }

        public async Task<Job?> GetJobByJobIdAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Job>(
                _Select_JobByJobId,
                new { JobId = jobId }
            );
        }

        public async Task<JobApplication?> GetJobApplicationAsync(int? userId, int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<JobApplication>(
                _Select_JobApplication,
                new
                {
                    UserId = userId,
                    JobId = jobId
                }
            );
        }

        public async Task ApplyJobAsync(JobApplication jobApplication)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Insert_JobApplication,
                jobApplication
            );
        }
    }
}
