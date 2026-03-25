using CeylonHire.Api.Models;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;
using System.Net.Http.Headers;

namespace CeylonHire.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Insert_NewUser;
        private readonly string _Insert_JobSeekerProfile;
        private readonly string _Select_UserByEmail;
        public UserRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Insert_NewUser = _queryLoader.Load("User", "Insert_NewUser.sql");
            _Insert_JobSeekerProfile = _queryLoader.Load("User", "Insert_JobSeekerProfile.sql");
            _Select_UserByEmail = _queryLoader.Load("User", "Select_UserByEmail.sql");
        }

        public async Task<int> RegisterNewJobseekerAsync(User newUser, JobSeekerProfile jobseekerProfile)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();

            using var transaction = db.BeginTransaction();
            try
            {
                var userId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewUser,
                    new
                    {
                        Email = newUser.Email,
                        PasswordHash = newUser.PasswordHash,
                        RoleId = newUser.RoleId
                    },
                    transaction
                );

                var jobseekerProfileId = await db.ExecuteScalarAsync<int>(
                    _Insert_JobSeekerProfile,
                    new
                    {
                        UserId = userId,
                        FirstName = jobseekerProfile.FirstName,
                        LastName = jobseekerProfile.LastName,
                        Gender = jobseekerProfile.Gender,
                        Address = jobseekerProfile.Address,
                        NIC = jobseekerProfile.NIC,
                        DateOfBirth = jobseekerProfile.DateOfBirth,
                        ExperienceYears = jobseekerProfile.ExperienceYears,
                        CVUrl = jobseekerProfile.CVUrl
                    },
                    transaction
                );
                transaction.Commit();
                return jobseekerProfileId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(
                _Select_UserByEmail,
                new { Email = email }
             );

        }
    }
}
