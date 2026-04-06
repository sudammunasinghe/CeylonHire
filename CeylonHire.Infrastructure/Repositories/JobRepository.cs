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
    public class JobRepository : IJobRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Select_JobMasterData;
        private readonly string _Select_CompanyDetailsByUserId;
        private readonly string _Insert_NewJob;
        private readonly string _Insert_JobSkills;
        public JobRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobMasterData = _queryLoader.Load("Job", "Select_JobMasterData.sql");
            _Select_CompanyDetailsByUserId = _queryLoader.Load("Job", "Select_CompanyDetailsByUserId.sql");
            _Insert_NewJob = _queryLoader.Load("Job", "Insert_NewJob.sql");
            _Insert_JobSkills = _queryLoader.Load("Job", "Insert_JobSkills.sql");
        }

        public async Task<JobMasterDataResult> GetJobMasterDataAsync()
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(_Select_JobMasterData);
            var result = new JobMasterDataResult
            {
                jobTypes = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                jobModes = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                experienceLevels = (await multi.ReadAsync<MasterDataEntity>()).ToList(),
                skills = (await multi.ReadAsync<MasterDataEntity>()).ToList()
            };
            return result;
        }

        public async Task<CompanyProfile?> GetCompanyDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyDetailsByUserId,
                new { UserId =  userId }
            );
        }

        public async Task CreateJobPostAsync(Job newJob, ICollection<int> skillIds)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                var jobId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewJob,
                    newJob,
                    transaction
                );

                foreach(var skill in skillIds)
                {
                    await db.ExecuteAsync(
                        _Insert_JobSkills,
                        new
                        {
                            JobId = jobId,
                            SkillId = skill
                        },
                        transaction
                    );
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
