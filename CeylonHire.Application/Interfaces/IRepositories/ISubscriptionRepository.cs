using CeylonHire.Application.DTOs.Subscription;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface ISubscriptionRepository
    {
        Task<JobSeekerProfile?> GetJobSeekerByUserIdAsync(int? userId);
        Task<CompanyProfile?> GetCompanyByCompanyIdAsync(int companyId);
        Task<CompanySubscription?> GetCompanySubscriptonAsync(int jobseekerId, int companyId);
        Task SubscribeCompanyAsync(CompanySubscription subscription);
        Task UnsubscribeCompanyAsync(CompanySubscription subscription);
        Task<List<int>> GetSubscribedUsersByCompanyIdAsync(int companyId);
    }
}
