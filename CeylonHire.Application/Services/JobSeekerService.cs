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
        private readonly IMasterDataService _masterDataService;
        public JobSeekerService(
            IJobSeekerRepository jobSeekerRepository,
            ICurrentUserService currentUserService,
            IMasterDataService masterDataService
            )
        {
            _jobSeekerRepository = jobSeekerRepository;
            _currentUserService = currentUserService;
            _masterDataService = masterDataService;
        }

        public async Task<JobSeekerProfileDto> GetCurrentJobSeekerProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var profile =
                await _jobSeekerRepository.GetCurrentJobSeekerProfileAsync(loggedUserId);

            if (profile.profileDetails == null)
                throw new NotFoundException("Job seeker profile not found.");

            return new JobSeekerProfileDto
            {
                Id = profile.profileDetails.Id,
                UserId = profile.profileDetails.UserId,
                FirstName = profile.profileDetails.FirstName,
                LastName = profile.profileDetails.LastName,
                Address = profile.profileDetails.Address,
                NIC = profile.profileDetails.NIC,
                DateOfBirth = profile.profileDetails.DateOfBirth,
                ExperienceYears = profile.profileDetails.ExperienceYears,
                CVUrl = profile.profileDetails.CVUrl,
                Skills = profile.userSkills.Select(x => x.SkillName).ToList(),
            };
        }

        public async Task UpdateCurrentJobSeekerProfileAsync(UpdateJobSeekerProfileDto dto)
        {
            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var profile =
                await _jobSeekerRepository.GetJobSeekerByJobSeekerProfileIdAsync(dto.Id);

            if (profile == null)
                throw new NotFoundException("Job seeker profile not found.");

            if (loggedUser != profile.UserId)
                throw new UnauthorizedAccessException("Access denied.");

            profile.Update(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.NIC,
                dto.ExperienceYears,
                dto.CVUrl
            );

            var masterData =
                await _masterDataService.GetJobMasterDataAsync();

            var validSkillIds = masterData.skills.Select(x => x.Id).ToHashSet();
            if (!dto.SkillIds.All(skill => validSkillIds.Contains(skill)))
                throw new BadRequestException("Invalid skill.");

            await _jobSeekerRepository.UpdateCurrentJobSeekerProfileAsync(profile, dto.SkillIds);
        }

        public async Task SaveJobAsync(int jobId)
        {
            var jobSeekerId = await GetJobSeekerIdAsync();

            if (jobSeekerId == null)
                throw new BadRequestException("Only job seekers can save jobs.");

            var job =
                await _jobSeekerRepository.GetJobByJobIdAsync(jobId);

            if (job == null)
                throw new NotFoundException("Job not found.");

            var savedJob =
                await _jobSeekerRepository.GetSavedJobAsync(jobSeekerId, jobId);

            if (savedJob != null)
            {
                if (savedJob.IsActive == true)
                    throw new ConflictException("Job already saved.");

                await _jobSeekerRepository.ReActivateSavedJobAsync(savedJob.Id);
                return;
            }
            await _jobSeekerRepository.SaveJobAsync(jobSeekerId, jobId);
        }

        public async Task UnsaveJobAsync(int jobId)
        {
            var jobSeekerId = await GetJobSeekerIdAsync();
            if (jobSeekerId == null)
                throw new BadRequestException("Only job seekers can unsave jobs.");

            var job =
                await _jobSeekerRepository.GetJobByJobIdAsync(jobId);

            if (job == null)
                throw new NotFoundException("Job not found.");

            var savedJob =
                await _jobSeekerRepository.GetSavedJobAsync(jobSeekerId, jobId);

            if (savedJob != null)
            {
                if (savedJob.IsActive == false)
                    throw new ConflictException("Job already unsaved.");
                await _jobSeekerRepository.UnsaveJobAsync(savedJob.Id);
                return;
            }
            throw new NotFoundException("Saved job not found.");
        }

        public async Task<IEnumerable<SavedJobDetailsDto>> GetSavedJobsAsync()
        {
            var jobSeekerId = await GetJobSeekerIdAsync();
            if (jobSeekerId == null)
                throw new BadRequestException("Only job seekers can retrieve saved jobs.");

            return await _jobSeekerRepository.GetSavedJobsAsync(jobSeekerId);
        }

        private async Task<int?> GetJobSeekerIdAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var jobSeeker =
                await _jobSeekerRepository.GetCurrentJobSeekerProfileAsync(loggedUserId);
            return jobSeeker.profileDetails?.Id;
        }
    }
}
