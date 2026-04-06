using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class JobMasterDataResult
    {
        public List<MasterDataEntity> jobTypes { get; set; }
        public List<MasterDataEntity> jobModes { get; set; }
        public List<MasterDataEntity> experienceLevels { get; set; }
        public List<MasterDataEntity> skills { get; set; }
    }
}
