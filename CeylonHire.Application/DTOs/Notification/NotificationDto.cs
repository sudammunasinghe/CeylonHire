namespace CeylonHire.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public int? NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? NotificationType { get; set; }
        public string? SentUser { get; set; }
        public string? ActionUrl { get; set; }
        public bool? IsRead { get; set; }
    }
}
