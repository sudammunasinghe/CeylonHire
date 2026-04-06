using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IJobRepository
    {
        Task<JobMasterDataResult> GetJobMasterDataAsync();
        Task<CompanyProfile?> GetCompanyDetailsByUserIdAsync(int userId);
        Task CreateJobPostAsync(Job newJob, ICollection<int>? skillIds);
    }
}
