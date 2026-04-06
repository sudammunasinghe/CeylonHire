using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Job
{
    public class JobDetailsDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Salary { get; set; }
        public string? Location { get; set; }
        public int? NumberOfOpenings { get; set; }
        public int? MinExperienceYears { get; set; }
        public int? JobTypeId { get; set; }
        public int? JobModeId { get; set; }
        public int ExperienceLevelId { get; set; }
        public DateTime DeadLine { get; set; }
        public ICollection<int>? SkillIds { get; set; }
    }
}
