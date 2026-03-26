using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Domain.Entities;
using CeylonHire.Infrastructure.Persistence;
using CeylonHire.Infrastructure.Persistence.Sql.Helpers;
using Dapper;

namespace CeylonHire.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;
        private readonly string _Insert_NewUser;
        private readonly string _Insert_JobSeekerProfile;
        private readonly string _Insert_CompanyProfile;
        private readonly string _Select_UserByEmail;
        public UserRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Insert_NewUser = _queryLoader.Load("User", "Insert_NewUser.sql");
            _Insert_JobSeekerProfile = _queryLoader.Load("JobSeeker", "Insert_JobSeekerProfile.sql");
            _Select_UserByEmail = _queryLoader.Load("User", "Select_UserByEmail.sql");
            _Insert_CompanyProfile = _queryLoader.Load("CompanyProfile", "Insert_CompanyProfile.sql");
        }

        /// <summary>
        /// Register a new jobseeker.
        /// </summary>
        /// <param name="newUser">An object containing user details.</param>
        /// <param name="jobseekerProfile">An object containing jobseeker profile details.</param>
        /// <returns>Returns the Id of the newly registered jobseeker.</returns>
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
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Register a new company.
        /// </summary>
        /// <param name="newUser">An object containing user details.</param>
        /// <param name="companyProfile">An object containing company profile details.</param>
        /// <returns>Returns the Id of the newly registered company.</returns>
        public async Task<int> RegisterNewCompanyAsync(User newUser, CompanyProfile companyProfile)
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

                var companyProfileId = await db.ExecuteScalarAsync<int>(
                    _Insert_CompanyProfile,
                    new
                    {
                        UserId = userId,
                        CompanyName = companyProfile.CompanyName,
                        Description = companyProfile.Description,
                        WebSite = companyProfile.WebSite,
                        LogoUrl = companyProfile.LogoUrl
                    },
                    transaction
                 );
                transaction.Commit();
                return companyProfileId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a user by email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByEmailAsync(string? email)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(
                _Select_UserByEmail,
                new { Email = email }
             );

        }
    }
}
