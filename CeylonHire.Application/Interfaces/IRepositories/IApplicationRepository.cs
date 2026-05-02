using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IApplicationRepository
    {
        Task<Job?> GetJobByJobIdAsync(int jobId);
        Task<JobApplication?> GetJobApplicationAsync(int? userId, int jobId);
        Task ApplyJobAsync(JobApplication jobApplication);
    }
}
