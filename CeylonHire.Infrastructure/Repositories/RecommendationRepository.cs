using CeylonHire.Application.DTOs.RecommendedUser;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Infrastructure.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Select_RecommendedUsersByJobId;

        public RecommendationRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_RecommendedUsersByJobId = _queryLoader.Load("Recommendation", "Select_RecommendedUsersByJobId.sql");
        }

        public async Task<List<RecommendedUserDto>> GetRecommendedUsersAsync(int jobId)
        {
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<RecommendedUserDto>(
                _Select_RecommendedUsersByJobId,
                new { JobId =  jobId }
            );
            return result.ToList();
        }
    }
}
