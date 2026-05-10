namespace CeylonHire.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int? SentUserId { get; set; }
        public int NotificationTypeId { get; set; }
        public int? IsActionable { get; set; }
        public int? ActionUrl { get; set; }
    }
}
