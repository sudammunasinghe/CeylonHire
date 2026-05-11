using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.DTOs.Subscription
{
    public class CompanySubscription : BaseEntity
    {
        public int Id { get; set; }
        public int JobseekerId { get; set; }
        public int CompanyId { get; set; }
    }
}
