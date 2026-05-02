using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;
        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        [HttpGet("job-master-date")]
        public async Task<ActionResult<ApiResponse<JobMasterDataResult>>> GetJobMasterDataAsync()
        {
            var result = await _masterDataService.GetJobMasterDataAsync();
            return Ok(new ApiResponse<JobMasterDataResult>
            {
                Success = true,
                Data = result,
                Message = "Master data retrieved successfully."
            });
        }
    }
}
