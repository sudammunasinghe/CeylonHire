using CeylonHire.Application.DTOs.CompanyProfile;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUserService _currentUserService;
        public CompanyService(ICompanyRepository companyRepository, ICurrentUserService currentUserService)
        {
            _companyRepository = companyRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CompanyProfileDto> GetCurrentCompanyProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var profile =
                await _companyRepository.GetCurrentCompanyProfileAsync(loggedUserId);

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
    }
}
