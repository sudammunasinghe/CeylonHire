using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CeylonHire.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<int>>> GetUnreadNotificationCount()
        {
            var count = await _notificationService.GetUnreadNotificationCount();
            return Ok( new ApiResponse<int>
            {
                Success = true,
                Data = count,
                Message = "Notification count retrieved successfully."
            });
        }
    }
}
