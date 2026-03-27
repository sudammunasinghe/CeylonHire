using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using CeylonHire.Domain.Enums;

namespace CeylonHire.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        public AuthService(IUserRepository userRepository, IPasswordService passwordService, ITokenGeneratorService tokenGeneratorService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenGeneratorService = tokenGeneratorService;
        }

        /// <summary>
        /// Register a new jobseeker with the provided details in the JobSeekerProfileDto.
        /// </summary>
        /// <param name="dto">An object conataining jobseeker profile details.</param>
        /// <returns></returns>
        public async Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto)
        {
            var newUser =
                await CreateNewUserAsync(dto.Email, dto.Password, RoleEnum.Jobseeker);

            var jobseekerProfileInfo = JobSeekerProfile.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.NIC,
                dto.ExperienceYears,
                dto.CVUrl
            );

            var jobseekerProfileId =
                await _userRepository.RegisterNewJobseekerAsync(newUser, jobseekerProfileInfo);

            if (jobseekerProfileId > 0)
                return _tokenGeneratorService.GenerateToken(newUser);
            return "Registration failed.";
        }


        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns></returns>
        public async Task<string> RegisterNewCompanyAsync(CompanyProfileDto dto)
        {
            var newUser =
                await CreateNewUserAsync(dto.Email, dto.Password, RoleEnum.Employer);

            var companyProfileInfo = CompanyProfile.Create(
                dto.CompanyName,
                dto.Description,
                dto.WebSite,
                dto.LogoUrl
            );

            var companyProfileId =
                await _userRepository.RegisterNewCompanyAsync(newUser, companyProfileInfo);

            if (companyProfileId > 0)
                return _tokenGeneratorService.GenerateToken(newUser);
            return "Registration failed.";
        }

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the credentials are invalid.</exception>
        public async Task<string> Login(string email, string password)
        {
            var user =
                await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid Credentials ...");
            return _tokenGeneratorService.GenerateToken(user);
        }

        /// <summary>
        /// Creates a new user with the specified email, password, and role.
        /// </summary>
        /// <param name="email">The email of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="role">The role of the new user.</param>
        /// <returns><see cref="User"/>An object containing the newly created user details.</returns>
        /// <exception cref="DupplicateEmailException">Thrown when the email already exists.</exception>
        private async Task<User> CreateNewUserAsync(string? email, string? password, RoleEnum role)
        {
            var user =
                    await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
                throw new DupplicateEmailException("Email already exists.");

            var newUser = User.Create(
                email,
                password,
                (int)role
            );
            newUser.PasswordHash = _passwordService.HashPassword(password);
            return newUser;
        }
    }
}
