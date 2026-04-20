using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IJobSeekerRepository
    {
        Task<(JobSeekerProfile? profileDetails, List<Skill>? userSkills)> GetCurrentJobSeekerProfileAsync(int userId);
        Task UpdateCurrentJobSeekerProfileAsync(JobSeekerProfile updatedProfile, List<int> updatedSkills);
        Task<JobSeekerProfile?> GetJobSeekerByJobSeekerProfileIdAsync(int jobSeekerProfileId);
    }
}
