using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.Auth;
using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Register a new jobseeker with the provided details in the JobSeekerProfileDto.
        /// </summary>
        /// <param name="dto">An object conataining jobseeker profile details.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [HttpPost("register/jobseeker")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewJobseekerAsync([FromBody] JobSeekerProfileDto dto)
        {
            var result = await _authService.RegisterNewJobseekerAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = result,
                Message = "Registration successful."
            });
        }

        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [HttpPost("register/company")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewCompanyAsync([FromBody] CompanyProfileDto dto)
        {
            var result = await _authService.RegisterNewCompanyAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = result,
                Message = "Registration successful."
            });
        }

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = token,
                Message = "Login successful."
            });
        }

        /// <summary>
        /// forgot password functionality for users who have forgotten their password. which typically involves sending a password reset link or code to the user's email address.
        /// </summary>
        /// <param name="dto">An object containing the user's email.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [EnableRateLimiting("ForgotPasswordPolicy")]
        [HttpPost("forgot-Password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        /// <summary>
        /// reset password functionality for users who have forgotten their password.
        /// </summary>
        /// <param name="dto">An object containing the user's new password, token & tokenId.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> ResetPasswordAsync([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPassword(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Password updated Successfully"
            });
        }

        /// <summary>
        /// change password functionality for authenticated users who want to change their password.
        /// </summary>
        /// <param name="dto">An object containing the user's current and new passwords.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
        {
            await _authService.ChangePasswordAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Password changed Successfully"
            });
        }
    }
}
