using CeylonHire.Application.DTOs.JobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IJobSeekerService
    {
        Task<JobSeekerProfileDto> GetCurrentJobSeekerProfileAsync();
    }
}
