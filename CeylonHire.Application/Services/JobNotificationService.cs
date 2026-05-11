using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Services
{
    public class JobNotificationService : IJobNotificationService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationService _notificationService;
        public JobNotificationService(ISubscriptionRepository subscriptionRepository, INotificationService notificationService)
        {
            _subscriptionRepository = subscriptionRepository;
            _notificationService = notificationService;
        }
        public async Task NotifyNewJobPostedAsync(string? jobTitle, int companyId, string? companyName)
        {
            var subscribers = await GetSubscribedUsersAsync(companyId);
            string title = "New Job Alert";
            string message = $"{companyName} has posted a new {jobTitle} job.";
            int notificationTypeId = 1;

            await _notificationService.SendNotificationAsync(title, message, notificationTypeId, subscribers);
        }

        private async Task<List<int>> GetSubscribedUsersAsync(int companyId)
        {
            return await _subscriptionRepository.GetSubscribedUsersByCompanyIdAsync(companyId);
        }
    }
}
