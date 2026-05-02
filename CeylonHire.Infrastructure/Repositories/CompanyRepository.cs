using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

namespace CeylonHire.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_CompanyProfileDetails;
        private readonly string _Update_CompanyProfile;
        public CompanyRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_CompanyProfileDetails = _queryLoader.Load("CompanyProfile", "Select_CompanyProfileDetails.sql");
            _Update_CompanyProfile = _queryLoader.Load("CompanyProfile", "Update_CompanyProfile.sql");
        }

        /// <summary>
        /// Gets the company profile details for a specific user and company asynchronously from the database.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="companyId">The Id of the company.</param>
        /// <returns>Returns the company profile details if found; otherwise, null.</returns>
        public async Task<CompanyProfile?> GetCompanyProfileDetailsAsync(int? userId, int? companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyProfileDetails,
                new
                {
                    CompanyId = companyId,
                    UserId = userId
                }
            );
        }

        /// <summary>
        /// Updates the current company's profile information asynchronously in the database.
        /// </summary>
        /// <param name="updatedProfile">The updated company profile data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateCurrentCompanyProfileAsync(CompanyProfile updatedProfile)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_CompanyProfile,
                new
                {
                    Id = updatedProfile.Id,
                    CompanyName = updatedProfile.CompanyName,
                    Description = updatedProfile.Description,
                    WebSite = updatedProfile.WebSite,
                    LogoUrl = updatedProfile.LogoUrl
                }
            );
        }
    }
}
