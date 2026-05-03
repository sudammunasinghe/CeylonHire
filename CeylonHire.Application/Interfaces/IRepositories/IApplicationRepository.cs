using CeylonHire.Domain.Entities;
using CeylonHire.Domain.Enums;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IApplicationRepository
    {
        Task<Job?> GetJobByJobIdAsync(int jobId);
        Task<JobApplication?> GetJobApplicationAsync(int? userId, int jobId);
        Task ApplyJobAsync(JobApplication jobApplication);
        Task ManageJobApplicationAsync(int? userId, JobApplication updatedApplication);
        Task<JobApplication?> GetJobApplicationByApplicationIdAsync(int applicationId);
        Task<CompanyProfile?> GetCompanyByJobIdAsync(int? jobId);
    }
}
