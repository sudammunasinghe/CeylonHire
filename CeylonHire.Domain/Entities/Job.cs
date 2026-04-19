
using CeylonHire.Domain.Exceptions;
namespace CeylonHire.Domain.Entities
{
    public class Job : BaseEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public decimal? Salary { get; set; }
        public string? Location { get; set; }
        public int? NumberOfOpenings { get; set; }
        public int? MinExperienceYears { get; set; }
        public int? JobTypeId { get; set; }
        public int? JobModeId { get; set; }
        public int ExperienceLevelId { get; set; }
        public DateTime DeadLine { get; private set; }
        private Job() { }

        public static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Job title is required.");
        }

        public static void ValidateDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Job description is required.");
        }

        public static void ValidateDeadLine(DateTime dueDate)
        {
            if (dueDate <= DateTime.Now)
                throw new DomainException("Invalid deadline.");
        }

        public static Job Create(
            int companyId,
            string? title,
            string? description,
            decimal? salary,
            string? location,
            int? noOfOpenings,
            int? minExperienceYears,
            int? jobTypeId,
            int? jobModeId,
            int expLevelId,
            DateTime deadLine
            )
        {
            ValidateTitle(title);
            ValidateDescription(description);
            ValidateDeadLine(deadLine);

            return new Job
            {
                CompanyId = companyId,
                Title = title,
                Description = description,
                Salary = salary,
                Location = location,
                NumberOfOpenings = noOfOpenings,
                MinExperienceYears = minExperienceYears,
                JobTypeId = jobTypeId,
                JobModeId = jobModeId,
                ExperienceLevelId = expLevelId,
                DeadLine = deadLine
            };
        }

        public void Update(
            int jobId,
            int companyId,
            string? title,
            string? description,
            decimal? salary,
            string? location,
            int? noOfOpenings,
            int? minExperienceYears,
            int? jobTypeId,
            int? jobModeId,
            int expLevelId,
            DateTime deadLine
           )
        {
            ValidateTitle(title);
            ValidateDescription(description);
            ValidateDeadLine(deadLine);

            Id = jobId;
            CompanyId = companyId;
            Title = title;
            Description = description;
            Salary = salary;
            Location = location;
            NumberOfOpenings = noOfOpenings;
            MinExperienceYears = minExperienceYears;
            JobTypeId = jobTypeId;
            JobModeId = jobModeId;
            ExperienceLevelId = expLevelId;
            DeadLine = deadLine;
        }

    }
}
