namespace CeylonHire.Domain.Entities
{
    public class NotificationRecipient : BaseEntity
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int RecipientUserId { get; set; }
        public bool IsRead { get; set; }
    }
}
