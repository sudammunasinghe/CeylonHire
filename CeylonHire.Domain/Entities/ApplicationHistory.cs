using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class ApplicationHistory : BaseEntity
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int StatusId { get; set; }
        public int ActionTriggeredUser { get; set; }
        public string? Remark { get; set; }
    }
}
