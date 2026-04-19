using CeylonHire.Application.DTOs.Job;
using CeylonHire.Application.DTOs.PagedResult;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IJobService
    {
        /// <summary>
        /// Creates a new job post based on the provided details in the CreateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be created.</param>
        /// <returns></returns>
        Task CreateJobPostAsync(CreateJobDetailsDto dto);

        /// <summary>
        /// Updates an existing job post with the provided details in the UpdateJobDetailsDto object.
        /// </summary>
        /// <param name="dto">An object containing the details of the job to be updated.</param>
        /// <returns></returns>
        Task UpdateJobAsync(UpdateJobDetailsDto dto);

        /// <summary>
        /// Inactivate a job post by its Id.
        /// </summary>
        /// <param name="jobId">The Id of the job to be inactivated.</param>
        /// <returns></returns>
        Task RemoveJobByIdAsync(int jobId);

        /// <summary>
        /// gets all the job posts created by the company.
        /// </summary>
        /// <returns>A list of <see cref="JobDetailsDto"/> objects representing the job posts.</returns>
        Task<IEnumerable<JobDetailsDto>> GetMyJobsAsync();

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
        /// <returns>A <see cref="JobDetailsDto"/> object containing the job details.</returns>
        Task<JobDetailsDto> GetJobDetailsByJobIdAsync(int jobId);
    }
}
