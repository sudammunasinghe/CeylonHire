using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.DTOs.PagedResult;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IJobRepository
    {
        /// <summary>
        /// returns the master data required for creating or updating a job post, including job types, job modes, experience levels, and skills.
        /// </summary>
        /// <returns>A <see cref="JobMasterDataResult"/> object containing the master data.</returns>
        Task<JobMasterDataResult> GetJobMasterDataAsync();

        /// <summary>
        /// gets the company details associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose company details are to be retrieved.</param>
        /// <returns>A <see cref="CompanyProfile"/> object containing the company details, or null if not found.</returns>
        Task<CompanyProfile?> GetCompanyDetailsByUserIdAsync(int userId);

        /// <summary>
        /// creates a new job post with the provided job details and associated skill IDs.
        /// </summary>
        /// <param name="newJob">An object containing the details of the job to be created.</param>
        /// <param name="skillIds">A collection of skill IDs associated with the job.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateJobPostAsync(Job newJob, ICollection<int>? skillIds);

        /// <summary>
        /// get job details by job id, including associated skills and company information.
        /// </summary>
        /// <param name="jobId">The ID of the job to be retrieved.</param>
        /// <returns>A <see cref="Job"/> object containing the job details, or null if not found.</returns>
        Task<Job?> GetJobByJobIdAsync(int jobId);

        /// <summary>
        /// updates an existing job post with the provided updated job details and associated skill IDs.
        /// </summary>
        /// <param name="updatedJob">An object containing the updated details of the job.</param>
        /// <param name="skillIds">A collection of skill IDs associated with the job.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateJobAsync(Job updatedJob, ICollection<int> skillIds);

        Task RemoveJobByIdAsync(int jobId);
        Task<IEnumerable<JobDetailsDto>> GetMyJobsAsync(int companyId);
        Task<PagedResult<JobDetailsDto>> GetAllJobsAsync(
            string? search,
            string? location,
            int? jobTypeId,
            int? jobModeId,
            int pageNumber,
            int pageSize
        );

        Task<JobDetailsDto?> GetJobDetailsByJobIdAsync(int jobId);
    }
}
