using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.Auth;
using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using CeylonHire.Domain.Enums;
using CeylonHire.Domain.ValueObjects;

namespace CeylonHire.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly IEmailService _emailService;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(
            IUserRepository userRepository,
            IHashingService hashingService,
            ITokenGeneratorService tokenGeneratorService,
            IEmailService emailService,
            ICurrentUserService currentUserService
            )
        {
            _userRepository = userRepository;
            _hashingService = hashingService;
            _tokenGeneratorService = tokenGeneratorService;
            _emailService = emailService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Register a new jobseeker with the provided details in the JobSeekerProfileDto.
        /// </summary>
        /// <param name="dto">An object conataining jobseeker profile details.</param>
        /// <returns>Returns a JWT token if the registration is successful.</returns>
        /// <exception cref="BadRequestException">Thrown when the requested data is invalid.</exception>
        public async Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Invalid Request Data ...");

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

            var userId = await _userRepository.RegisterNewJobseekerAsync(newUser, jobseekerProfileInfo);
            newUser.Id = userId;
            return _tokenGeneratorService.GenerateJwtToken(newUser);
        }


        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns>Returns a JWT token if the registration is successful.</returns>
        /// <exception cref="BadRequestException">Thrown when the requested data is invalid.</exception>
        public async Task<string> RegisterNewCompanyAsync(CompanyProfileDto dto)
        {
            if (dto == null)
                throw new BadRequestException("Invalid Request Data ...");

            var newUser =
                await CreateNewUserAsync(dto.Email, dto.Password, RoleEnum.Employer);

            var companyProfileInfo = CompanyProfile.Create(
                dto.CompanyName,
                dto.Description,
                dto.WebSite,
                dto.LogoUrl
            );

            var userId = await _userRepository.RegisterNewCompanyAsync(newUser, companyProfileInfo);
            newUser.Id = userId;
            return _tokenGeneratorService.GenerateJwtToken(newUser);
        }

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the credentials are invalid.</exception>
        /// <exception cref="BadRequestException">Thrown when the email or password is not provided.</exception>
        public async Task<string> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new BadRequestException("Email and password are required ...");

            var user =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null || !_hashingService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid Credentials ...");
            return _tokenGeneratorService.GenerateJwtToken(user);
        }

        /// <summary>
        /// forgot password functionality for users who have forgotten their password. which typically involves sending a password reset link or code to the user's email address.
        /// </summary>
        /// <param name="dto">An object containing the user's email.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        /// <exception cref="BadRequestException">Thrown when the email is not provided.</exception>
        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new BadRequestException("Email is required ...");

            var user =
                    await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return "If the email exists, a reset link has been sent...";

            var token = _tokenGeneratorService.GeneratePasswordResetToken();
            var tokenId = Guid.NewGuid();
            var hashToken = _hashingService.HashPasswordResetToken(token);
            var expiry = DateTime.Now.AddMinutes(10);

            var resetLink = $"https://ceylonhire.com/reset-password?token={token}&tokenId={tokenId}";

            var subject = "Password Reset Request";
            var body = $@"
                    <!DOCTYPE html>
                        <html>
                            <head>
                                <meta charset='UTF-8'>
                            </head>
                            <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'>
                                <div style='max-width:600px; margin:auto; background:white; padding:20px; border-radius:10px;'>                            
                                    <h2 style='color:#333;'>🔐 Password Reset Request</h2>                            
                                    <p>Hello,</p>                          
                                    <p>We received a request to reset your password for your <strong>CeylonHire</strong> account.</p>                            
                                    <p>Click the button below to reset your password:</p>                            
                                    <div style='text-align:center; margin:30px 0;'>
                                        <a href='{resetLink}' 
                                           style='background-color:#007bff; color:white; padding:12px 25px; text-decoration:none; border-radius:5px; display:inline-block;'>
                                           Reset Password
                                        </a>
                                    </div>                    
                                    <p>This link will expire in <strong>10 minutes</strong>.</p>                   
                                    <p>If you did not request a password reset, please ignore this email.</p>                   
                                    <hr style='margin:30px 0;'>                   
                                    <p style='font-size:12px; color:gray;'>
                                        If the button doesn't work, copy and paste this link into your browser:<br>
                                        <a href='{resetLink}'>{resetLink}</a>
                                    </p>                    
                                    <p style='margin-top:20px;'>Thanks,<br><strong>CeylonHire Team</strong></p>                   
                                </div>                   
                            </body>
                        </html>
                    ";

            await _userRepository.SavePasswordResetTokenAsync(user.Id, tokenId, hashToken, expiry);
            await _emailService.SendEmailAsync(
                    user.Email,
                    subject,
                    body
                );
            return "If the email exists, a reset link has been sent...";
        }

        /// <summary>
        /// reset password functionality for users who have forgotten their password.
        /// </summary>
        /// <param name="dto">An object containing the user's new password, token & tokenId.</param>
        /// <returns>Returns true if the password was successfully reset, otherwise false.</returns>
        /// <exception cref="BadRequestException">Thrown when the reset token is invalid or expired.</exception>
        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var user =
                await _userRepository.GetUserByPasswordResetTokenIdAsync(dto.TokenId);

            if (user == null ||
               user.PasswordResetTokenHash == null ||
               !_hashingService.VerifyPasswordResetToken(dto.Token, user.PasswordResetTokenHash) ||
               user.PasswordResetTokenExpiry < DateTime.Now
            )
            {
                throw new BadRequestException("Invalid or expired reset token ...");
            }

            Password validatedPassword = new(dto.NewPassword);
            var passwordHash = _hashingService.HashPassword(validatedPassword.Value);
            user.ChangePassword(passwordHash);
            await _userRepository.UpdatePasswordAsync(user);
        }

        /// <summary>
        /// change password functionality for authenticated users who want to change their password.
        /// </summary>
        /// <param name="dto">An object containing the user's current and new passwords.</param>
        /// <returns>Returns true if the password was successfully changed, otherwise false.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authenticated.</exception>
        /// <exception cref="BadRequestException">Thrown when the current password is invalid or the new password is the same as the current password.</exception>
        /// <exception cref="BadRequestException">Thrown when the current password or new password is empty.</exception>
        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
                throw new BadRequestException("Current password and new password cannot be empty ...");

            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized ...");

            var loggedUser =
                await _userRepository.GetUserByUserIdAsync(loggedUserId);

            if (loggedUser == null)
                throw new UnauthorizedAccessException("User not found ...");

            if (!_hashingService.VerifyPassword(dto.CurrentPassword, loggedUser.PasswordHash))
                throw new BadRequestException("Invalid current password ...");

            if (dto.NewPassword == dto.CurrentPassword)
                throw new BadRequestException("New password should be differ from the current password ...");

            Password validatedPassword = new(dto.NewPassword);
            var passwordHash = _hashingService.HashPassword(validatedPassword.Value);
            loggedUser.ChangePassword(passwordHash);
            await _userRepository.UpdatePasswordAsync(loggedUser);
        }

        /// <summary>
        /// Creates a new user with the specified email, password, and role.
        /// </summary>
        /// <param name="email">The email of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="role">The role of the new user.</param>
        /// <returns><see cref="User"/>An object containing the newly created user details.</returns>
        /// <exception cref="DuplicateEmailException">Thrown when the email already exists.</exception>
        private async Task<User> CreateNewUserAsync(string email, string password, RoleEnum role)
        {
            var user =
                    await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
                throw new DuplicateEmailException("Email already exists.");

            Password validatedPassword = new(password);
            var passwordHash = _hashingService.HashPassword(validatedPassword.Value);
            var newUser = User.Create(
                email,
                passwordHash,
                (int)role
            );
            return newUser;
        }
    }
}
