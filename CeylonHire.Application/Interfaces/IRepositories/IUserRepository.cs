using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Register a new jobseeker.
        /// </summary>
        /// <param name="user">An object containing user details.</param>
        /// <param name="jobseekerProfile">An object containing jobseeker profile details.</param>
        /// <returns>Returns the Id of the newly registered jobseeker.</returns>
        Task<int> RegisterNewJobseekerAsync(User user, JobSeekerProfile jobseekerProfile);

        /// <summary>
        /// Register a new company.
        /// </summary>
        /// <param name="user">An object containing user details.</param>
        /// <param name="companyProfile">An object containing company profile details.</param>
        /// <returns>Returns the Id of the newly registered company.</returns>
        Task<int> RegisterNewCompanyAsync(User user, CompanyProfile companyProfile);

        /// <summary>
        /// Get user details by user's email.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>Returns the user details if found, otherwise null.</returns>
        Task<User?> GetUserByEmailAsync(string? email);
    }
}
