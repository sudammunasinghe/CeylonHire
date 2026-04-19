using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.JobSeeker;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;
        public JobSeekerController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<JobSeekerProfileDto>>> GetCurrentJobSeekerProfileAsync()
        {
            var result = await _jobSeekerService.GetCurrentJobSeekerProfileAsync();
            return Ok(new ApiResponse<JobSeekerProfileDto>
            {
                Success = true,
                Data = result,
                Message = "Profile details retrieved successfully."
            });
        }
    }
}
