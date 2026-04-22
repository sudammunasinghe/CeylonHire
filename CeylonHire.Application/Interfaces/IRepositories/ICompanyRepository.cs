using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        Task<CompanyProfile?> GetCurrentCompanyProfileAsync(int userId);
    }
}
