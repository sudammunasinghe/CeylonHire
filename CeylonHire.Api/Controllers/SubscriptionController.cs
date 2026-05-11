using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPut("subscribe/{companyId}")]
        public async Task<ActionResult<ApiResponse<string>>> SubscribeCompanyAsync(int companyId)
        {
            await _subscriptionService.SubscribeCompanyAsync(companyId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Successfully subscribed."
            });
        }

        [HttpPut("unsubscribe/{companyId}")]
        public async Task<ActionResult<ApiResponse<string>>> UnsubscribeCompanyAsync(int companyId)
        {
            await _subscriptionService.UnsubscribeCompanyAsync(companyId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Successfully unsubscribed."
            });
        }
    }
}
