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
        public async Task<ActionResult<ApiResponse<int>>> GetUnreadNotificationCountAsync()
        {
            var count = await _notificationService.GetUnreadNotificationCountAsync();
            return Ok(new ApiResponse<int>
            {
                Success = true,
                Data = count,
                Message = "Notification count retrieved successfully."
            });
        }

        [HttpPut("{id}/read")]
        public async Task<ActionResult<ApiResponse<string>>> MarkNotificationAsReadAsync(int id)
        {
            await _notificationService.MarkNotificationAsReadAsync(id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Notification marked as read successfully."
            });
        }

        [HttpPut("read-all")]
        public async Task<ActionResult<ApiResponse<string>>> MarkAllNotificationsAsRead()
        {
            await _notificationService.MarkAllNotificationsAsRead();
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "All notifications marked as read successfully."
            });
        }
    }
}
