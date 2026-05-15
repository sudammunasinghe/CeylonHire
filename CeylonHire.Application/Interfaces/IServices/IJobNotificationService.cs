using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IJobNotificationService
    {
        Task NotifyNewJobPostedAsync(int jobId, string? jobTitle, int companyId, string? companyName, List<int> JobSkills);
    }
}
