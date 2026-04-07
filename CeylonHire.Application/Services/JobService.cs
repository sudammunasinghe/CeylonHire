using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICurrentUserService _currentUserService;
        public JobService(IJobRepository jobRepository, ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Creates a new job post based on the provided details in the CreateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be created.</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to create a job.</exception>
        /// <exception cref="NotFoundException">Thrown when the company profile is not found.</exception>
        public async Task CreateJobPostAsync(CreateJobDetailsDto dto)
        {
            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var company =
                await _jobRepository.GetCompanyDetailsByUserIdAsync(loggedUser);

            if (company == null)
                throw new NotFoundException("Company profile not found.");

            await ValidateJobMasterDataAsync(dto.JobTypeId, dto.JobModeId, dto.ExperienceLevelId, dto.SkillIds);

            var newJob = Job.Create(
                company.Id,
                dto.Title,
                dto.Description,
                dto.Salary,
                dto.Location,
                dto.NumberOfOpenings,
                dto.MinExperienceYears,
                dto.JobTypeId,
                dto.JobModeId,
                dto.ExperienceLevelId,
                dto.DeadLine
            );

            await _jobRepository.CreateJobPostAsync(newJob, dto.SkillIds);
        }

        /// <summary>
        /// Updates an existing job post with the provided details in the UpdateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be updated.</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to update the job.</exception>
        /// <exception cref="NotFoundException">Thrown when the job or company profile is not found.</exception>
        public async Task UpdateJobAsync(UpdateJobDetailsDto dto)
        {
            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var company =
                await _jobRepository.GetCompanyDetailsByUserIdAsync(loggedUser);

            if (company == null)
                throw new NotFoundException("Company profile not found.");

            var job =
                await _jobRepository.GetJobByJobIdAsync(dto.Id);

            if (job == null)
                throw new NotFoundException("Job not found.");

            if (job.CompanyId != company.Id)
                throw new UnauthorizedAccessException("You are not allowed to update this job.");

            await ValidateJobMasterDataAsync(dto.JobTypeId, dto.JobModeId, dto.ExperienceLevelId, dto.SkillIds);

            job.Update(
                dto.Id,
                company.Id,
                dto.Title,
                dto.Description,
                dto.Salary,
                dto.Location,
                dto.NumberOfOpenings,
                dto.MinExperienceYears,
                dto.JobTypeId,
                dto.JobModeId,
                dto.ExperienceLevelId,
                dto.DeadLine
            );

            await _jobRepository.UpdateJobAsync(job, dto.SkillIds);
        }

        /// <summary>
        /// validates the provided job master data (job type, job mode, experience level, and skills) against the master data stored in the database.
        /// </summary>
        /// <param name="jobTypeId">The ID of the job type to be validated.</param>
        /// <param name="jobModeId">The ID of the job mode to be validated.</param>
        /// <param name="expLevelId">The ID of the experience level to be validated.</param>
        /// <param name="skillIds">A collection of skill IDs to be validated.</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">Thrown when any of the provided job master data is invalid.</exception>
        private async Task ValidateJobMasterDataAsync(int? jobTypeId, int? jobModeId, int? expLevelId, ICollection<int>? skillIds)
        {
            var masterData =
                await _jobRepository.GetJobMasterDataAsync();

            if (skillIds == null || !skillIds.Any())
                throw new BadRequestException("At least one skill is required.");

            var jobTypeIds = masterData.jobTypes.Select(x => x.Id).ToHashSet();
            var jobModeIds = masterData.jobModes.Select(x => x.Id).ToHashSet();
            var expLevelIds = masterData.experienceLevels.Select(x => x.Id).ToHashSet();
            var validSkillIds = masterData.skills.Select(sk => sk.Id).ToHashSet();

            if (jobTypeId.HasValue && !jobTypeIds.Contains(jobTypeId.Value))
            {
                throw new BadRequestException("Invalid job type.");
            }

            if (jobModeId.HasValue && !jobModeIds.Contains(jobModeId.Value))
            {
                throw new BadRequestException("Invalid job mode.");
            }

            if (expLevelId.HasValue && !expLevelIds.Contains(expLevelId.Value))
            {
                throw new BadRequestException("Invalid experience level.");
            }

            if (!skillIds.All(skillId => validSkillIds.Contains(skillId)))
            {
                throw new BadRequestException("Invalid skill.");
            }
        }
    }
}
