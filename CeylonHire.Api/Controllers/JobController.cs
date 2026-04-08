using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
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

        /// <summary>
        /// Creates a new job post based on the provided details in the CreateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be created.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateJobPostAsync(CreateJobDetailsDto dto)
        {
            await _jobService.CreateJobPostAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job created successfully."
            });
        }

        /// <summary>
        /// Updates an existing job post with the provided details in the UpdateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be updated.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<string>>> UpdateJobAsync(UpdateJobDetailsDto dto)
        {
            await _jobService.UpdateJobAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job updated successfully."
            });
        }

        [HttpPut("{jobId}/courseInactivate")]
        public async Task<ActionResult<ApiResponse<string>>> RemoveJobByIdAsync(int jobId)
        {
            await _jobService.RemoveJobByIdAsync(jobId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Job removed successfully."
            });
        }

        [HttpGet("company")]
        public async Task<ActionResult<ApiResponse<IEnumerable<JobDetailsDto>>>> GetMyJobsAsync()
        {
            var result = await _jobService.GetMyJobsAsync();
            return Ok(new ApiResponse<IEnumerable<JobDetailsDto>>{
                Success = true,
                Data = result,
                Message = "Retrieved jobs successfully."
            });
        }
    }
}
