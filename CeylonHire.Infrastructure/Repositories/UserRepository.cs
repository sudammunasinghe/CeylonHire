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
        private readonly string _Update_UserForSavePasswordResetToken;
        private readonly string _Select_UserByPasswordResetTokenId;
        private readonly string _Update_UserForResetPassword;
        private readonly string _Select_UserByUserId;
        public UserRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Insert_NewUser = _queryLoader.Load("User", "Insert_NewUser.sql");
            _Insert_JobSeekerProfile = _queryLoader.Load("JobSeeker", "Insert_JobSeekerProfile.sql");
            _Select_UserByEmail = _queryLoader.Load("User", "Select_UserByEmail.sql");
            _Insert_CompanyProfile = _queryLoader.Load("CompanyProfile", "Insert_CompanyProfile.sql");
            _Update_UserForSavePasswordResetToken = _queryLoader.Load("User", "Update_UserForSavePasswordResetToken.sql");
            _Select_UserByPasswordResetTokenId = _queryLoader.Load("User", "Select_UserByPasswordResetTokenId.sql");
            _Update_UserForResetPassword = _queryLoader.Load("User", "Update_UserForResetPassword.sql");
            _Select_UserByUserId = _queryLoader.Load("User", "Select_UserByUserId.sql");
        }

        /// <summary>
        /// Register a new jobseeker.
        /// </summary>
        /// <param name="newUser">An object containing user details.</param>
        /// <param name="jobseekerProfile">An object containing jobseeker profile details.</param>
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

                await db.ExecuteAsync(
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
                return userId;
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

                await db.ExecuteAsync(
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
                return userId;
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

        /// <summary>
        /// Get user details by password reset token Id.
        /// </summary>
        /// <param name="tokenId">The Id of the password reset token.</param>
        /// <returns>Returns the user details if found, otherwise null.</returns>
        public async Task<User?> GetUserByPasswordResetTokenIdAsync(Guid? tokenId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(
                _Select_UserByPasswordResetTokenId,
                new { TokenId = tokenId }
            );
        }

        /// <summary>
        /// update the password hash of a user in the database.
        /// </summary>
        /// <param name="user">The user whose password needs to be updated.</param>
        /// <returns>Returns the number of affected rows.</returns>
        public async Task UpdatePasswordAsync(User user)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_UserForResetPassword,
                new
                {
                    UserId = user.Id,
                    PasswordHash = user.PasswordHash
                }
            );

        }

        /// <summary>
        /// saves a password reset token for a user, along with its expiry time.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="expiry">The expiry time of the token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SavePasswordResetTokenAsync(int userId, Guid tokenId, string tokenHash, DateTime expiry)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(
                _Update_UserForSavePasswordResetToken,
                new
                {
                    UserId = userId,
                    TokenId = tokenId,
                    TokenHash = tokenHash,
                    Expiry = expiry
                }
            );
        }

        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise,
        /// null.</returns>
        public async Task<User?> GetUserByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(
                _Select_UserByUserId,
                new { UserId = userId }
            );
        }
    }
}
