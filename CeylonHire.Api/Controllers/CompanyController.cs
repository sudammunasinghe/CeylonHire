using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Get the current company's profile details.
        /// </summary>
        /// <returns>Returns the current company's profile details.</returns>
        [HttpGet("profile/me")]
        public async Task<ActionResult<ApiResponse<CompanyProfileDto>>> GetCurrentCompanyProfileAsync()
        {
            var result = await _companyService.GetCurrentCompanyProfileAsync();
            return Ok(new ApiResponse<CompanyProfileDto>
            {
                Success = true,
                Data = result,
                Message = "Profile details retrieved successfully."
            });
        }

        /// <summary>
        /// Update the current company's profile details.
        /// </summary>
        /// <param name="dto">The company's profile details to update.</param>
        /// <returns>Returns a success message if the update is successful</returns>
        [HttpPut("update-profile")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateCurrentCompanyProfileAsync([FromBody] CompanyProfileDto dto)
        {
            await _companyService.UpdateCurrentCompanyProfileAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Profile updated successfully."
            });
        }
    }
}
