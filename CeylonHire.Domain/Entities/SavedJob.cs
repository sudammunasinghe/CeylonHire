namespace CeylonHire.Domain.Entities
{
    public class SavedJob : BaseEntity
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int JobId { get; set; }
    }
}
