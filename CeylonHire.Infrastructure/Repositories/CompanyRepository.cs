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
        public CompanyRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_CompanyProfileDetails = _queryLoader.Load("CompanyProfile", "Select_CompanyProfileDetails.sql");
        }

        public async Task<CompanyProfile?> GetCurrentCompanyProfileAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CompanyProfile>(
                _Select_CompanyProfileDetails,
                new { UserId = userId }
            );
        }
    }
}
