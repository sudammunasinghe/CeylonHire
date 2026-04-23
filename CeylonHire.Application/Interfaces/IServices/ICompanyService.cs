using CeylonHire.Application.DTOs.CompanyProfile;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ICompanyService
    {
        /// <summary>
        /// Get the current company's profile details.
        /// </summary>
        /// <returns>Returns the current company's profile details.</returns>
        Task<CompanyProfileDto> GetCurrentCompanyProfileAsync();

        /// <summary>
        /// Update the current company's profile details.
        /// </summary>
        /// <param name="dto">The company's profile details to update.</param>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        Task UpdateCurrentCompanyProfileAsync(CompanyProfileDto dto);
    }
}
