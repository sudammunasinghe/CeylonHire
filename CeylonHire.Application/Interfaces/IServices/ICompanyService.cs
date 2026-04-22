using CeylonHire.Application.DTOs.CompanyProfile;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ICompanyService
    {
        Task<CompanyProfileDto> GetCurrentCompanyProfileAsync();
    }
}
