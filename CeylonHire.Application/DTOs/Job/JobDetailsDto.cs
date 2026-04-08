using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Job
{
    public class JobDetailsDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public decimal? Salary { get; set; }
        public string? Location { get; set; }
        public int? NumberOfOpenings { get; set; }
        public int? MinExperienceYears { get; set; }
        public string? JobType { get; set; }
        public string? JobMode { get; set; }
        public string? ExperienceLevel { get; set; }
        public DateTime DeadLine { get; private set; }
    }
}
