namespace CeylonHire.Application.DTOs.JobSeeker
{
    public class SavedJobDetailsDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public string? JobMode { get; set; }
        public DateTime? PostedDate { get; set; }
    }
}
