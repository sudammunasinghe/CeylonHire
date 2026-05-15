using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.Application;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> ApplyJobAsync([FromForm] JobApplicationRequest request)
        {
            var applicationDetails = new ApplicationDto
            {
                JobId = request.JobId,
                CV = new FileDto
                {
                    FileStream = request?.CVFile?.OpenReadStream(),
                    FileName = request?.CVFile?.FileName,
                },
                CoverLetter = request?.CoverLetterFile != null ? new FileDto
                {
                    FileStream = request?.CoverLetterFile?.OpenReadStream(),
                    FileName = request?.CoverLetterFile?.FileName,
                } : null
            };
            await _applicationService.ApplyJobAsync(applicationDetails);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Successfully applied to job."
            });
        }

        [HttpPut("{applicationId}/{status}")]
        public async Task<ActionResult<ApiResponse<string>>> ManageJobApplicationAsync(int applicationId, ApplicationStatusEnum status)
        {
            await _applicationService.ManageJobApplicationAsync(applicationId, status);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Application updated successfully."
            });
        }
    }
}
