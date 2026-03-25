using CeylonHire.Api.Models;
using CeylonHire.Api.Models.JobSeeker;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto);
    }
}
