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

        [HttpPut("update-profile")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateCurrentCompanyProfileAsync(CompanyProfileDto dto)
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
