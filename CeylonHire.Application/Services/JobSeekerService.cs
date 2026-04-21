using CeylonHire.Application.DTOs.JobSeeker;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Application.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJobRepository _jobRepository;
        public JobSeekerService(IJobSeekerRepository jobSeekerRepository, ICurrentUserService currentUserService, IJobRepository jobRepository)
        {
            _jobSeekerRepository = jobSeekerRepository;
            _currentUserService = currentUserService;
            _jobRepository = jobRepository;
        }

        public async Task<JobSeekerProfileDto> GetCurrentJobSeekerProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var result =
                await _jobSeekerRepository.GetCurrentJobSeekerProfileAsync(loggedUserId);

            if (result.profileDetails == null)
                throw new NotFoundException("Profile not found.");

            return new JobSeekerProfileDto
            {
                Id = result.profileDetails.Id,
                UserId = result.profileDetails.UserId,
                FullName = $"{result.profileDetails.FirstName} {result.profileDetails.LastName}",
                Address = result.profileDetails.Address,
                NIC = result.profileDetails.NIC,
                DateOfBirth = result.profileDetails.DateOfBirth,
                ExperienceYears = result.profileDetails.ExperienceYears,
                CVUrl = result.profileDetails.CVUrl,
                Skills = result.userSkills.Select(x => x.SkillName).ToList(),
            };
        }

        public async Task UpdateCurrentJobSeekerProfileAsync(UpdateJobSeekerProfileDto dto)
        {
            var profile =
                await _jobSeekerRepository.GetJobSeekerByJobSeekerProfileIdAsync(dto.Id);

            if (profile == null)
                throw new NotFoundException("Job seeker profile not found.");

            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null || loggedUserId != profile.UserId)
                throw new UnauthorizedAccessException("Unauthorized.");

            profile.Update(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.NIC,
                dto.ExperienceYears,
                dto.CVUrl
            );

            var masterData =
                await _jobRepository.GetJobMasterDataAsync();

            var validSkillIds = masterData.skills.Select(x => x.Id).ToHashSet();
            if (!dto.SkillIds.All(skill => validSkillIds.Contains(skill)))
                throw new BadRequestException("Invalid skill.");

            await _jobSeekerRepository.UpdateCurrentJobSeekerProfileAsync(profile, dto.SkillIds);
        }
    }
}
