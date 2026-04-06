using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateJobPostAsync(JobDetailsDto dto)
        {
            var masterData =
                await _jobRepository.GetJobMasterDataAsync();

            var loggedUser =_currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var company = 
                await _jobRepository.GetCompanyDetailsByUserIdAsync(loggedUser);

            var jobTypeIds = masterData.jobTypes.Select(x => x.Id).ToHashSet();
            var jobModeIds = masterData.jobModes.Select(x => x.Id).ToHashSet();
            var expLevelIds = masterData.experienceLevels.Select(x => x.Id).ToHashSet();
            var validSkillIds = masterData.skills.Select(sk => sk.Id).ToHashSet();

            if(dto.JobTypeId.HasValue && !jobTypeIds.Contains(dto.JobTypeId.Value))
            {
                throw new BadRequestException("Invalid job type.");
            }

            if(dto.JobModeId.HasValue && !jobModeIds.Contains(dto.JobModeId.Value))
            {
                throw new BadRequestException("Invalid job mode.");
            }

            if(!expLevelIds.Contains(dto.ExperienceLevelId))
            {
                throw new BadRequestException("Invalid experience level.");
            }

            if (!dto.SkillIds.All(skillId => validSkillIds.Contains(skillId)))
            {
                throw new BadRequestException("Invalid skill.");
            }

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
    }
}
