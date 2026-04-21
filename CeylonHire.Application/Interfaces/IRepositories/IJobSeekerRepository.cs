using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IJobSeekerRepository
    {
        Task<(JobSeekerProfile? profileDetails, List<Skill>? userSkills)> GetCurrentJobSeekerProfileAsync(int userId);
        Task UpdateCurrentJobSeekerProfileAsync(JobSeekerProfile updatedProfile, List<int> updatedSkills);
        Task<JobSeekerProfile?> GetJobSeekerByJobSeekerProfileIdAsync(int jobSeekerProfileId);
        Task<SavedJob?> GetSavedJobAsync(int jobSeekerId, int jobId);
        Task ReActivateSavedJobAsync(int savedJobId);
        Task SaveJobAsync(int jobSeekerId, int jobId);
    }
}
