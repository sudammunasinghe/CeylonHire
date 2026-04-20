using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.JobSeeker
{
    public class UpdateJobSeekerProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public int? ExperienceYears { get; set; }
        public string? CVUrl { get; set; }
        public List<int>? SkillIds { get; set; }
    }
}
