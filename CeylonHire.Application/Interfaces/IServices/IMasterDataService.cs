using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IMasterDataService
    {
        Task<JobMasterDataResult?> GetJobMasterDataAsync();
    }
}
