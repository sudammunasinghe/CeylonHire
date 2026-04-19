using CeylonHire.Application.DTOs.JobSeeker;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly ICurrentUserService _currentUserService;
        public JobSeekerService(IJobSeekerRepository jobSeekerRepository, ICurrentUserService currentUserService)
        {
            _jobSeekerRepository = jobSeekerRepository;
            _currentUserService = currentUserService;
        }

        public async Task<JobSeekerProfileDto> GetCurrentJobSeekerProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var result =
                await _jobSeekerRepository.GetCurrentJobSeekerProfileAsync(loggedUserId);

            if (result == null)
                throw new NotFoundException("Profile not found.");
            
            return new JobSeekerProfileDto
            {
                Id = result.
            }
        }
    }
}
