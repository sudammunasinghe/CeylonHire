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
        private readonly IRecommendationRepository _recommendationRepository;
        public JobNotificationService(ISubscriptionRepository subscriptionRepository, INotificationService notificationService, IRecommendationRepository recommendationRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _notificationService = notificationService;
            _recommendationRepository = recommendationRepository;
        }
        public async Task NotifyNewJobPostedAsync(int jobId, string? jobTitle, int companyId, string? companyName, List<int> JobSkills)
        {
            var subscribers = await GetSubscribedUsersAsync(companyId);
            var recommendedUsers = await GetRecommededUsersAsync(jobId, JobSkills);

            var allUsers = subscribers
                .Union(recommendedUsers)
                .Distinct()
                .ToList();

            string title = "New Job Alert";
            string message = $"{companyName} has posted a new {jobTitle} job.";
            int notificationTypeId = 1;

            await _notificationService.SendNotificationAsync(title, message, notificationTypeId, allUsers);
        }

        private async Task<List<int>> GetSubscribedUsersAsync(int companyId)
        {
            return await _subscriptionRepository.GetSubscribedUsersByCompanyIdAsync(companyId);
        }

        private async Task<List<int>> GetRecommededUsersAsync(int jobId, List<int> JobSkills)
        {
            var jobSkillsCount = JobSkills.Count;
            var matchingPercentage = 40;
            var recommendedUsers = await _recommendationRepository.GetRecommendedUsersAsync(jobId);
            return recommendedUsers
                .Where(x => ((double)x.MatchCount/jobSkillsCount * 100) >= matchingPercentage)
                .OrderByDescending(x => x.MatchCount)
                .Select(x => x.UserId)
                .Take(20)
                .ToList();
        }
    }
}
