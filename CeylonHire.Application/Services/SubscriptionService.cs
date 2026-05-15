using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using System.ComponentModel.DataAnnotations;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.DTOs.Subscription;

namespace CeylonHire.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICurrentUserService _currentUserService;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, ICurrentUserService currentUserService)
        {
            _subscriptionRepository = subscriptionRepository;
            _currentUserService = currentUserService;
        }

        public async Task SubscribeCompanyAsync(int companyId)
        {
            var loggedUser = _currentUserService.UserId;

            var jobseeker =
                await _subscriptionRepository.GetJobSeekerByUserIdAsync(loggedUser);
            if (jobseeker == null)
                throw new NotFoundException("Jobseeker not found.");

            var company =
                await _subscriptionRepository.GetCompanyByCompanyIdAsync(companyId);
            if (company == null)
                throw new NotFoundException("Company not found.");

            var companySubscription =
                await _subscriptionRepository.GetCompanySubscriptonAsync(jobseeker.Id, company.Id);

            if (companySubscription != null)
            {
                if(companySubscription.IsActive == true)
                    throw new ConflictException($"You already subscribed {company.CompanyName}");
                companySubscription.IsActive = true;
                companySubscription.LastModifiedDateTime = DateTime.UtcNow;

            }
            else
            {
                companySubscription = new CompanySubscription
                {
                    JobseekerId = jobseeker.Id,
                    CompanyId = companyId,
                };
            }
            await _subscriptionRepository.SubscribeCompanyAsync(companySubscription);
        }

        public async Task UnsubscribeCompanyAsync(int companyId)
        {
            var loggedUser = _currentUserService.UserId;

            var jobseeker =
                await _subscriptionRepository.GetJobSeekerByUserIdAsync(loggedUser);
            if (jobseeker == null)
                throw new NotFoundException("Jobseeker not found.");

            var company =
                await _subscriptionRepository.GetCompanyByCompanyIdAsync(companyId);
            if (company == null)
                throw new NotFoundException("Company not found.");

            var companySubscription =
                await _subscriptionRepository.GetCompanySubscriptonAsync(jobseeker.Id, company.Id);

            if (companySubscription == null)
                throw new NotFoundException("No subscription found.");

            if (companySubscription.IsActive == false)
                throw new ConflictException("You have already unsubscribed.");

            companySubscription.IsActive = false;
            companySubscription.LastModifiedDateTime = DateTime.UtcNow;
            await _subscriptionRepository.UnsubscribeCompanyAsync(companySubscription);
        }
    }
}
