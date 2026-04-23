using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System.ComponentModel.Design;

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
            _Select_CompanyProfileDetails = _queryLoader.Load("Job", "Select_CompanyDetails.sql");
            _Update_CompanyProfile = _queryLoader.Load("CompanyProfile", "Update_CompanyProfile.sql");
        }

        public async Task UpdateCurrentCompanyProfileAsync(CompanyProfile updatedProfile)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_CompanyProfile,
                updatedProfile
            );
        }

        public async Task<CompanyProfile?> GetCompanyProfileByIdAsync(int companyId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyProfileDetails,
                new 
                { 
                    CompanyId = companyId,
                    UserId = (int?)null
                }
            );
        }
    }
}
