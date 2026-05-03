using CeylonHire.Application.DTOs.Application;
using CeylonHire.Domain.Enums;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IApplicationService
    {
        Task ApplyJobAsync(ApplicationDto dto);
        Task ManageJobApplicationAsync(int applicationId, ApplicationStatusEnum status);
    }
}
