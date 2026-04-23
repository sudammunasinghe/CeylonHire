using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using System.Reflection.Metadata.Ecma335;

namespace CeylonHire.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJobRepository _jobRepository;
        public CompanyService(ICompanyRepository companyRepository, ICurrentUserService currentUserService, IJobRepository jobRepository)
        {
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
            _jobRepository = jobRepository;
        }

        public async Task<CompanyProfileDto> GetCurrentCompanyProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var profile =
                await _jobRepository.GetCompanyDetailsByUserIdAsync(loggedUserId);

            if (profile == null)
                throw new NotFoundException("Company profile not found.");

            return new CompanyProfileDto
            {
                Id = profile.Id,
                CompanyName = profile.CompanyName,
                Description = profile.Description,
                WebSite = profile.WebSite,
                LogoUrl = profile.LogoUrl,
            };

        }

        public async Task UpdateCurrentCompanyProfileAsync(CompanyProfileDto dto)
        {
            var profile = 
                await _companyRepository.GetCompanyProfileByIdAsync(dto.Id);

            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null || profile?.UserId != loggedUser)
                throw new UnauthorizedAccessException("Unauthorized.");

            profile.Update(
                dto.CompanyName,
                dto.Description,
                dto.WebSite,
                dto.LogoUrl
            );
            await _companyRepository.UpdateCurrentCompanyProfileAsync(profile);
        }
    }
}
