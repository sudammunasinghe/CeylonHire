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

        /// <summary>
        /// Get the current company's profile details.
        /// </summary>
        /// <returns>Returns the current company's profile details.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not logged in.</exception>
        /// <exception cref="NotFoundException">Thrown when the company profile is not found.</exception>
        public async Task<CompanyProfileDto> GetCurrentCompanyProfileAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized.");

            var profile =
                await _companyRepository.GetCompanyProfileDetailsAsync(loggedUserId, null);

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

        /// <summary>
        /// Update the current company's profile details.
        /// </summary>
        /// <param name="dto">The company's profile details to update.</param>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not logged in or does not have access.</exception>
        /// <exception cref="NotFoundException">Thrown when the company profile is not found.</exception>
        public async Task UpdateCurrentCompanyProfileAsync(CompanyProfileDto dto)
        {
            var loggedUser = _currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("User not logged In.");

            var profile =
                await _companyRepository.GetCompanyProfileDetailsAsync(null, dto.Id);

            if (profile == null)
                throw new NotFoundException("Profile not found.");

            if (loggedUser != profile.UserId)
                throw new UnauthorizedAccessException("Access denied.");

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
