using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewJobseekerAsync([FromForm] JobSeekerProfileDto dto)
        {
            var result = await _authService.RegisterNewJobseekerAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        /// <summary>
        /// Register a new company with the provided details in the CompanyProfileDto.
        /// </summary>
        /// <param name="dto">An object containing company profile details.</param>
        /// <returns><see cref="ApiResponse{string}"/></returns>
        [HttpPost("register/company")]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewCompanyAsync([FromForm] CompanyProfileDto dto)
        {
            var result = await _authService.RegisterNewCompanyAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        /// <summary>
        /// Authenticates a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns a JWT token if the login is successful.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(string email, string password)
        {
            var token = await _authService.Login(email, password);
            return Ok(new { tokenId = token });
        }
    }
}
