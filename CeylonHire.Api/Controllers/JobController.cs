using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateJobPostAsync(JobDetailsDto dto)
        {
            await _jobService.CreateJobPostAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job created successfully."
            });
        }

    }
}
