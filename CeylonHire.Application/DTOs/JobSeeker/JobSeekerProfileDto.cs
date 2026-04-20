using CeylonHire.Api.Models.JobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.JobSeeker
{
    public class JobSeekerProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ExperienceYears { get; set; }
        public string? CVUrl { get; set; }
        public List<string>? Skills { get; set; }
    }
}
