using CeylonHire.Application.DTOs.JobSeeker;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

namespace CeylonHire.Infrastructure.Repositories
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_JobSeekerProfileDetails;
        private readonly string _Update_JobSeekerProfile;
        private readonly string _Select_ExistingUserSkills;
        private readonly string _Update_UserSkills;
        private readonly string _Insert_UserSkill;
        private readonly string _Select_JobSeekerProfileById;
        private readonly string _Select_SavedJobAsync;
        private readonly string _Update_SavedJob;
        private readonly string _Insert_SavedJob;
        private readonly string _Select_SavedJobDetails;
        private readonly string _Select_JobByJobId;

        public JobSeekerRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobSeekerProfileDetails = _queryLoader.Load("JobSeeker", "Select_JobSeekerProfileDetails.sql");
            _Update_JobSeekerProfile = _queryLoader.Load("JobSeeker", "Update_JobSeekerProfile.sql");
            _Select_ExistingUserSkills = _queryLoader.Load("JobSeeker", "Select_ExistingUserSkills.sql");
            _Update_UserSkills = _queryLoader.Load("JobSeeker", "Update_UserSkills.sql");
            _Insert_UserSkill = _queryLoader.Load("JobSeeker", "Insert_UserSkill.sql");
            _Select_JobSeekerProfileById = _queryLoader.Load("JobSeeker", "Select_JobSeekerProfileById.sql");
            _Select_SavedJobAsync = _queryLoader.Load("JobSeeker", "Select_SavedJobAsync.sql");
            _Update_SavedJob = _queryLoader.Load("JobSeeker", "Update_SavedJob.sql");
            _Insert_SavedJob = _queryLoader.Load("JobSeeker", "Insert_SavedJob.sql");
            _Select_SavedJobDetails = _queryLoader.Load("JobSeeker", "Select_SavedJobDetails.sql");
            _Select_JobByJobId = _queryLoader.Load("JobSeeker", "Select_JobByJobId.sql");
        }

        public async Task<(JobSeekerProfile? profileDetails, List<Skill>? userSkills)> GetCurrentJobSeekerProfileAsync(int? userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(
                _Select_JobSeekerProfileDetails,
                new { UserId = userId }
            );

            var profileData = await multi.ReadFirstOrDefaultAsync<JobSeekerProfile>();
            var skills = (await multi.ReadAsync<Skill>()).ToList();
            return (profileData, skills);
        }

        public async Task UpdateCurrentJobSeekerProfileAsync(JobSeekerProfile updatedProfile, List<int> updatedSkills)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();
            try
            {
                await db.ExecuteAsync(
                    _Update_JobSeekerProfile,
                    updatedProfile,
                    transaction
                );

                var existingSkills = (await db.QueryAsync<int>(
                    _Select_ExistingUserSkills,
                    new { UserId = updatedProfile.UserId },
                    transaction
                )).ToList();

                var skillsToInactivate = existingSkills.Except(updatedSkills).ToList();
                var skillsToInsert = updatedSkills.Except(existingSkills).ToList();

                if (skillsToInactivate.Any())
                {
                    await db.ExecuteAsync(
                        _Update_UserSkills,
                        new
                        {
                            UserId = updatedProfile.UserId,
                            SkillIds = skillsToInactivate
                        },
                        transaction
                    );
                }

                if (skillsToInsert.Any())
                {
                    await db.ExecuteAsync(
                        _Insert_UserSkill,
                        skillsToInsert.Select(skill => new
                        {
                            UserId = updatedProfile.UserId,
                            SkillId = skill
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

        public async Task<JobSeekerProfile?> GetJobSeekerByJobSeekerProfileIdAsync(int jobSeekerProfileId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<JobSeekerProfile>(
                _Select_JobSeekerProfileById,
                new { JobSeekerProfileId = jobSeekerProfileId }
            );
        }

        public async Task<SavedJob?> GetSavedJobAsync(int? jobSeekerId, int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<SavedJob>(
                _Select_SavedJobAsync,
                new
                {
                    JobSeekerId = jobSeekerId,
                    JobId = jobId
                }
            );
        }

        public async Task ReActivateSavedJobAsync(int savedJobId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_SavedJob,
                new
                {
                    SavedJobId = savedJobId,
                    IsActive = true
                }
            );
        }

        public async Task SaveJobAsync(int? jobSeekerId, int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Insert_SavedJob,
                new
                {
                    JobSeekerId = jobSeekerId,
                    JobId = jobId
                }
            );
        }

        public async Task UnsaveJobAsync(int savedJobId)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_SavedJob,
                new
                {
                    SavedJobId = savedJobId,
                    IsActive = false
                }
            );
        }

        public async Task<IEnumerable<SavedJobDetailsDto>> GetSavedJobsAsync(int? jobSeekerId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<SavedJobDetailsDto>(
                _Select_SavedJobDetails,
                new { JobSeekerId = jobSeekerId }
            );
        }

        public async Task<Job?> GetJobByJobIdAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Job>(
                _Select_JobByJobId,
                new { JobId = jobId }
            );
        }
    }
}
