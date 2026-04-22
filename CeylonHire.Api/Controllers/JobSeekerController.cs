using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.JobSeeker;
using CeylonHire.Application.Interfaces.IServices;
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

        [HttpGet("profile")]
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

        [HttpPut("update-profile")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateCurrentJobSeekerProfileAsync(UpdateJobSeekerProfileDto dto)
        {
            await _jobSeekerService.UpdateCurrentJobSeekerProfileAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Profile updated successfully."
            });
        }

        [HttpPost("saved-jobs/{jobId}")]
        public async Task<ActionResult<ApiResponse<string>>> SaveJobAsync(int jobId)
        {
            await _jobSeekerService.SaveJobAsync(jobId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job saved successfully."
            });
        }

        [HttpPut("unsaved-jobs/{jobId}")]
        public async Task<ActionResult<ApiResponse<string>>> UnsaveJobAsync(int jobId)
        {
            await _jobSeekerService.UnsaveJobAsync(jobId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job unsaved successfully."
            });
        }

        [HttpGet("saved-jobs")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SavedJobDetailsDto>>>> GetSavedJobsAsync()
        {
            var result = await _jobSeekerService.GetSavedJobsAsync();
            return Ok(new ApiResponse<IEnumerable<SavedJobDetailsDto>>
            {
                Success = true,
                Data = result,
                Message = "Saved jobs retrieved successfully."
            });
        }
    }
}
