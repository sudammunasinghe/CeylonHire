using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Gets the company profile details for a given user and company.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="companyId">The Id of the company.</param>
        /// <returns>Returns the company profile details if found; otherwise, null.</returns>
        Task<CompanyProfile?> GetCompanyProfileDetailsAsync(int? userId, int? companyId);

        /// <summary>
        /// Updates the profile information for the current company asynchronously.
        /// </summary>
        /// <param name="updatedProfile">The updated company profile data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateCurrentCompanyProfileAsync(CompanyProfile updatedProfile);
    }
}
