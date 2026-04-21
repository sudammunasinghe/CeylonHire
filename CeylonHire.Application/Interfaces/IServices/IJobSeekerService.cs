using CeylonHire.Application.DTOs.JobSeeker;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IJobSeekerService
    {
        Task<JobSeekerProfileDto> GetCurrentJobSeekerProfileAsync();
        Task UpdateCurrentJobSeekerProfileAsync(UpdateJobSeekerProfileDto dto);
    }
}
