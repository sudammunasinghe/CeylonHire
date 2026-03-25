using CeylonHire.Api.Models;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<int> RegisterNewJobseekerAsync(User user, JobSeekerProfile jobseekerProfile);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
