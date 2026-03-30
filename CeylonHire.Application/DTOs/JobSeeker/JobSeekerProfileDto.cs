namespace CeylonHire.Api.Models.JobSeeker
{
    public class JobSeekerProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public int? ExperienceYears { get; set; }
        public string? CVUrl { get; set; }
    }
}
