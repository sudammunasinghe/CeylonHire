using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class SavedJob : BaseEntity
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int JobId { get; set; }
    }
}
