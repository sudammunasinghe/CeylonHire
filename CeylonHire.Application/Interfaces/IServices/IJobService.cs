using CeylonHire.Application.DTOs.Job;

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
    }
}
