using CeylonHire.Application.DTOs.ApiResponse;
using CeylonHire.Application.DTOs.Notification;
using CeylonHire.Application.DTOs.PagedResult;
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

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<PagedResult<NotificationDto>>>> GetNotificationsAsync(int pageNumber = 1, int pageSize = 20)
        {
            var result = await _notificationService.GetNotificationsAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<NotificationDto>>
            {
                Success = true,
                Data = result,
                Message = "Notifications retrieved successfully."
            });
        }

        [HttpGet("un-read")]
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

        [HttpPut("{id}/remove")]
        public async Task<ActionResult<ApiResponse<string>>> RemoveNotificationAsync(int id)
        {
            await _notificationService.RemoveNotificationAsync(id);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Notification is removed successfully."
            });
        }
    }
}
