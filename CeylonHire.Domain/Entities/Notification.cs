namespace CeylonHire.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public int Message { get; set; }
        public int SenderUserId { get; set; }
        public int NotificationTypeId { get; set; }
        public int IsActionable { get; set; }
        public int? ActionUrl { get; set; }
    }
}
