using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class MasterDataEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
