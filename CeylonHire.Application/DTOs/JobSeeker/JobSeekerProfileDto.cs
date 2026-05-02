namespace CeylonHire.Application.DTOs.JobSeeker
{
    public class JobSeekerProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ExperienceYears { get; set; }
        public string? CVUrl { get; set; }
        public List<string>? Skills { get; set; }
    }
}
