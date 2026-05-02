using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.DTOs.PagedResult;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IJobRepository
    {
        /// <summary>
        /// gets the company details associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose company details are to be retrieved.</param>
        /// <returns>A <see cref="CompanyProfile"/> object containing the company details, or null if not found.</returns>
        Task<CompanyProfile?> GetCompanyDetailsByUserIdAsync(int? userId);

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

        /// <summary>
        /// inactivates a job post by its Id.
        /// </summary>
        /// <param name="jobId">The Id of the job to be inactivated.</param>
        /// <returns></returns>
        Task RemoveJobByIdAsync(int jobId);

        /// <summary>
        /// Gets all the job posts created by the specified company.
        /// </summary>
        /// <param name="companyId">The Id of the company whose job posts are to be retrieved.</param>
        /// <returns>A list of <see cref="JobDetailsDto"/> objects representing the job posts.</returns>
        Task<IEnumerable<JobDetailsDto>> GetMyJobsAsync(int companyId);

        /// <summary>
        /// Gets all the job posts with pagination and filtering options for search, location, job type, and job mode.
        /// </summary>
        /// <param name="search">search value.</param>
        /// <param name="location">location value.</param>
        /// <param name="jobTypeId">job type Id.</param>
        /// <param name="jobModeId">job mode Id.</param>
        /// <param name="pageNumber">page number.</param>
        /// <param name="pageSize">page size.</param>
        /// <returns>A paged result of <see cref="JobDetailsDto"/> objects representing the job posts.</returns>
        Task<PagedResult<JobDetailsDto>> GetAllJobsAsync(
            string? search,
            string? location,
            int? jobTypeId,
            int? jobModeId,
            int pageNumber,
            int pageSize
        );

        /// <summary>
        /// retrieves the details of a specific job post by its Id.
        /// </summary>
        /// <param name="jobId">The Id of the job to be retrieved.</param>
        /// <returns>A <see cref="JobDetailsDto"/> object containing the job details, or null if not found.</returns>
        Task<JobDetailsDto?> GetJobDetailsByJobIdAsync(int jobId);
    }
}
