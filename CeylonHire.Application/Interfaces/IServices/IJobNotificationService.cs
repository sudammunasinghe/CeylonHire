using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IJobNotificationService
    {
        Task NotifyNewJobPostedAsync(string? jobTitle, int companyId, string? companyName);
    }
}
