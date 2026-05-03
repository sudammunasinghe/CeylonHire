using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Domain.Enums;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

namespace CeylonHire.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Select_JobByJobId;
        private readonly string _Select_JobApplication;
        private readonly string _Insert_JobApplication;
        private readonly string _Insert_ApplicationHistory;
        private readonly string _Select_JobApplicationByApplicationId;
        private readonly string _Update_JobApplication;
        private readonly string _Select_CompanyByJobId;

        public ApplicationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobByJobId = _queryLoader.Load("Application", "Select_JobByJobId.sql");
            _Select_JobApplication = _queryLoader.Load("Application", "Select_JobApplication.sql");
            _Insert_JobApplication = _queryLoader.Load("Application", "Insert_JobApplication.sql");
            _Insert_ApplicationHistory = _queryLoader.Load("Application", "Insert_ApplicationHistory.sql");
            _Select_JobApplicationByApplicationId = _queryLoader.Load("Application", "Select_JobApplicationByApplicationId.sql");
            _Update_JobApplication = _queryLoader.Load("Application", "Update_JobApplication.sql");
            _Select_CompanyByJobId = _queryLoader.Load("Application", "Select_CompanyByJobId.sql");
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
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                var applicationId = await db.ExecuteScalarAsync<int>(
                    _Insert_JobApplication,
                    jobApplication,
                    transaction
                );

                await db.ExecuteAsync(
                    _Insert_ApplicationHistory,
                    new
                    {
                        ApplicationId = applicationId,
                        Status = jobApplication.Status,
                        ActionTriggeredUser = jobApplication.UserId,
                        Remark = (string?)null
                    },
                    transaction
                );
                transaction.Commit();
            }
            catch { 
                transaction.Rollback();
                throw;
            }
        }

        public async Task ManageJobApplicationAsync(int? userId, JobApplication updatedApplication)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                await db.ExecuteAsync(
                    _Update_JobApplication,
                    updatedApplication,
                    transaction
                );

                await db.ExecuteAsync(
                    _Insert_ApplicationHistory,
                    new
                    {
                        ApplicationId = updatedApplication.Id,
                        Status = updatedApplication.Status,
                        ActionTriggeredUser = userId,
                        Remark = (string?)null
                    },
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

        public async Task<JobApplication?> GetJobApplicationByApplicationIdAsync(int applicationId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<JobApplication>(
                _Select_JobApplicationByApplicationId,
                new { ApplicationId = applicationId }
            );
        }

        public async Task<CompanyProfile?> GetCompanyByJobIdAsync(int? jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyByJobId,
                new { JobId = jobId }
            );
        }
    }
}
