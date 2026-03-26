using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.CompanyProfile;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// Register a new jobseeker with the provided details in the JobSeekerProfileDto.
        /// </summary>
        /// <param name="dto">An object conataining jobseeker profile details.</param>
        /// <returns></returns>
        Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto);

        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns></returns>
        Task<string> RegisterNewCompanyAsync(CompanyProfileDto dto);
    }
}
