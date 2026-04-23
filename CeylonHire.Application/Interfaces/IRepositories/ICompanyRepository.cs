using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        Task UpdateCurrentCompanyProfileAsync(CompanyProfile updatedProfile);
        Task<CompanyProfile?> GetCompanyProfileByIdAsync(int companyId);
    }
}
