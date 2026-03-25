namespace CeylonHire.Api.Models
{
    public class BaseEntity
    {
        public bool? IsActive { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
