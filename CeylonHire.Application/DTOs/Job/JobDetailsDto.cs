namespace CeylonHire.Application.DTOs.Job
{
    public class JobDetailsDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Salary { get; set; }
        public string? Location { get; set; }
        public int? NumberOfOpenings { get; set; }
        public int? MinExperienceYears { get; set; }
        public string? JobType { get; set; }
        public string? JobMode { get; set; }
        public string? ExperienceLevel { get; set; }
        public DateTime DeadLine { get; set; }
        public List<string>? JobSkill { get; set; }
    }
}
