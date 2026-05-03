using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IRepositories
{
    public interface IMasterDataRepository
    {
        Task<JobMasterDataResult?> GetJobMasterDataAsync();
    }
}
