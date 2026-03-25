using CeylonHire.Api.Models;
using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.DTOs.ApiResponse;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewJobseekerAsync([FromForm] JobSeekerProfileDto dto)
        {
            var result = await _authService.RegisterNewJobseekerAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }
    }
}
