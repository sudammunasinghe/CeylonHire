using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.DTOs.PagedResult;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

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
        private readonly string _Select_JobByJobId;
        private readonly string _Update_JobDetails;
        private readonly string _Select_ExistingSkillIds;
        private readonly string _Update_JobSkills;
        private readonly string _Update_JobForInactivation;
        private readonly string _Select_JobsByCompanyId;
        private readonly string _Select_AllJobs;
        private readonly string _Select_JobDetailsByJobId;
        public JobRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobMasterData = _queryLoader.Load("Job", "Select_JobMasterData.sql");
            _Select_CompanyDetailsByUserId = _queryLoader.Load("Job", "Select_CompanyDetailsByUserId.sql");
            _Insert_NewJob = _queryLoader.Load("Job", "Insert_NewJob.sql");
            _Insert_JobSkills = _queryLoader.Load("Job", "Insert_JobSkills.sql");
            _Select_JobByJobId = _queryLoader.Load("Job", "Select_JobByJobId.sql");
            _Update_JobDetails = _queryLoader.Load("Job", "Update_JobDetails.sql");
            _Select_ExistingSkillIds = _queryLoader.Load("Job", "Select_ExistingSkillIds.sql");
            _Update_JobSkills = _queryLoader.Load("Job", "Update_JobSkills.sql");
            _Update_JobForInactivation = _queryLoader.Load("Job", "Update_JobForInactivation.sql");
            _Select_JobsByCompanyId = _queryLoader.Load("Job", "Select_JobsByCompanyId.sql");
            _Select_AllJobs = _queryLoader.Load("Job", "Select_AllJobs.sql");
            _Select_JobDetailsByJobId = _queryLoader.Load("Job", "Select_JobDetailsByJobId.sql");
        }

        /// <summary>
        /// gets the master data required for job posting form such as job types, job modes, experience levels and skills.
        /// </summary>
        /// <returns>A <see cref="JobMasterDataResult"/> object containing the master data.</returns>
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

        /// <summary>
        /// gets the company details associated with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose company details are to be retrieved.</param>
        /// <returns>A <see cref="CompanyProfile"/> object containing the company details, or null if not found.</returns>
        public async Task<CompanyProfile?> GetCompanyDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyDetailsByUserId,
                new { UserId = userId }
            );
        }

        /// <summary>
        /// creates a new job post in the database and associates it with the specified skill IDs.
        /// </summary>
        /// <param name="newJob">An object containing the details of the job to be created.</param>
        /// <param name="skillIds">A collection of skill IDs associated with the job.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

                await db.ExecuteAsync(
                    _Insert_JobSkills,
                    skillIds.Select(skill => new
                    {
                        JobId = jobId,
                        SkillId = skill
                    }),
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

        /// <summary>
        /// gets the details of a job post by its ID.
        /// </summary>
        /// <param name="jobId">The ID of the job to be retrieved.</param>
        /// <returns>A <see cref="Job"/> object containing the job details, or null if not found.</returns>
        public async Task<Job?> GetJobByJobIdAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Job>(
                _Select_JobByJobId,
                new { JobId = jobId }
            );
        }

        /// <summary>
        /// updates the details of an existing job post in the database and manages the associations with skill IDs.
        /// </summary>
        /// <param name="updatedJob">An object containing the updated details of the job.</param>
        /// <param name="skillIds">A collection of skill IDs associated with the job.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateJobAsync(Job updatedJob, ICollection<int> skillIds)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                await db.ExecuteAsync(
                    _Update_JobDetails,
                    updatedJob,
                    transaction
                );

                skillIds ??= new List<int>();

                var existingSkillIds = (await db.QueryAsync<int>(
                    _Select_ExistingSkillIds,
                    new { JobId = updatedJob.Id },
                    transaction
                )).ToList();

                var IdsToIncativate = existingSkillIds.Except(skillIds).ToList();
                if (IdsToIncativate.Any())
                {
                    await db.ExecuteAsync(
                        _Update_JobSkills,
                        new
                        {
                            SkillIds = IdsToIncativate,
                            JobId = updatedJob.Id
                        },
                        transaction
                    );
                }

                var IdsToInsert = skillIds.Except(existingSkillIds).ToList();
                if (IdsToInsert.Any())
                {
                    await db.ExecuteAsync(
                        _Insert_JobSkills,
                        IdsToInsert.Select(skill => new
                        {
                            JobId = updatedJob.Id,
                            SkillId = skill,
                        }),
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

        /// <summary>
        /// Inactivate a job post by its Id.
        /// </summary>
        /// <param name="jobId">The Id of the job to be inactivated.</param>
        /// <returns></returns>
        public async Task RemoveJobByIdAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_JobForInactivation,
                new { JobId = jobId }
            );
        }

        /// <summary>
        /// Gets all the job posts created by the specified company.
        /// </summary>
        /// <param name="companyId">The Id of the company whose job posts are to be retrieved.</param>
        /// <returns>A list of <see cref="JobDetailsDto"/> objects representing the job posts.</returns>
        public async Task<IEnumerable<JobDetailsDto>> GetMyJobsAsync(int companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<JobDetailsDto>(
                _Select_JobsByCompanyId,
                new { CompanyId = companyId }
            );
        }

        /// <summary>
        /// Gets all the job posts with pagination and filtering options for search, location, job type, and job mode.
        /// </summary>
        /// <param name="search">search value.</param>
        /// <param name="location">location value.</param>
        /// <param name="jobTypeId">job type Id.</param>
        /// <param name="jobModeId">job mode Id.</param>
        /// <param name="pageNumber">page number.</param>
        /// <param name="pageSize">page size.</param>
        /// <returns>A paged result of <see cref="JobDetailsDto"/> objects representing the job posts.</returns>
        public async Task<PagedResult<JobDetailsDto>> GetAllJobsAsync(
            string? search,
            string? location,
            int? jobTypeId,
            int? jobModeId,
            int pageNumber,
            int pageSize
            )
        {
            var offset = (pageNumber - 1) * pageSize;
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(
                _Select_AllJobs,
                new
                {
                    Search = search,
                    Location = location,
                    JobTypeId = jobTypeId,
                    JobModeId = jobModeId,
                    PageSize = pageSize,
                    Offset = offset
                }
            );
            var items = (await multi.ReadAsync<JobDetailsDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<JobDetailsDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            };
        }

        /// <summary>
        /// retrieves the details of a specific job post by its Id.
        /// </summary>
        /// <param name="jobId">The Id of the job to be retrieved.</param>
        /// <returns>A <see cref="JobDetailsDto"/> object containing the job details, or null if not found.</returns>
        public async Task<JobDetailsDto?> GetJobDetailsByJobIdAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(
                _Select_JobDetailsByJobId,
                new { JobId = jobId }
            );

            var job = await multi.ReadFirstOrDefaultAsync<JobDetailsDto>();
            if (job == null)
                return null;

            var skill = (await multi.ReadAsync<string>()).ToList();
            job.JobSkill = skill;
            return job;
        }
    }
}
