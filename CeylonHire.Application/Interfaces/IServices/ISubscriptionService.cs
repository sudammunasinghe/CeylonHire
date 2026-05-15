namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ISubscriptionService
    {
        Task SubscribeCompanyAsync(int companyId);
        Task UnsubscribeCompanyAsync(int companyId);
    }
}
