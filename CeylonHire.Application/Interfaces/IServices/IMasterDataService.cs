using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IMasterDataService
    {
        Task<JobMasterDataResult?> GetJobMasterDataAsync();
    }
}
