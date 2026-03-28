using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.Auth;
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
        private readonly IHashingService _hashingService;
        private readonly IEmailService _emailService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        public AuthService(IUserRepository userRepository, IHashingService hashingService, ITokenGeneratorService tokenGeneratorService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _hashingService = hashingService;
            _tokenGeneratorService = tokenGeneratorService;
            _emailService = emailService;
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
                return _tokenGeneratorService.GenerateJwtToken(newUser);
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
                return _tokenGeneratorService.GenerateJwtToken(newUser);
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

            if (user == null || !_hashingService.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid Credentials ...");
            return _tokenGeneratorService.GenerateJwtToken(user);
        }

        /// <summary>
        /// forgot password functionality for users who have forgotten their password. which typically involves sending a password reset link or code to the user's email address.
        /// </summary>
        /// <param name="dto">An object containing the user's email.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user =
                    await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return "If the email exists, a reset link has been sent...";

            var token = _tokenGeneratorService.GeneratePasswordResetToken();
            var hashToken = _hashingService.HashPasswordResetToken(token);
            var expiry = DateTime.Now.AddMinutes(10);

            var resetLink = "https://ceylonhire.com/reset-password?token={token}&email={user.Email}";
            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Request",
                $"Click here to reset your password : {resetLink}"
            );

            await _userRepository.SavePasswordResetTokenAsync(user.Id, hashToken, expiry);
            return "If the email exists, a reset link has been sent...";
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
            newUser.PasswordHash = _hashingService.HashPassword(password);
            return newUser;
        }
    }
}
