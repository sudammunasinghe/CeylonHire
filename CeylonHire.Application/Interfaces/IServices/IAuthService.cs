using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.Auth;
using CeylonHire.Application.DTOs.CompanyProfile;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// Register a new jobseeker with the provided details in the JobSeekerProfileDto.
        /// </summary>
        /// <param name="dto">An object conataining jobseeker profile details.</param>
        /// <returns>Returns a JWT token if the registration is successful.</returns>
        Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto);

        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns>Returns a JWT token if the registration is successful.</returns>
        Task<string> RegisterNewCompanyAsync(CompanyProfileDto dto);

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        Task<string> LoginAsync(LoginDto dto);

        /// <summary>
        /// forgot password functionality for users who have forgotten their password. which typically involves sending a password reset link or code to the user's email address.
        /// </summary>
        /// <param name="dto">An object containing the user's email.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);

        /// <summary>
        /// reset password functionality for users who have forgotten their password.
        /// </summary>
        /// <param name="dto">An object containing the user's new password, token & tokenId.</param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordDto dto);

        /// <summary>
        /// change password functionality for authenticated users who want to change their password.
        /// </summary>
        /// <param name="dto">An object containing the user's current and new passwords.</param>
        /// <returns>Returns true if the password was successfully changed, otherwise false</returns>
        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}
