using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
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
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_JobSeekerProfileDetails;

        public JobSeekerRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_JobSeekerProfileDetails = _queryLoader.Load("JobSeeker", "Select_JobSeekerProfileDetails.sql");
        }

        public async Task<(JobSeekerProfile? profileDetails, List<Skill>? userSkills)> GetCurrentJobSeekerProfileAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var multi = await db.QueryMultipleAsync(
                _Select_JobSeekerProfileDetails,
                new { UserId =  userId }
            );

            var profileData = await multi.ReadFirstOrDefaultAsync<JobSeekerProfile>();
            var skills = (await multi.ReadAsync<Skill>()).ToList();
            return (profileData, skills);
        }
    }
}
