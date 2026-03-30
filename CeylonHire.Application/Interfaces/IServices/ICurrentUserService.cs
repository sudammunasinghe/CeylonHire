namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? Role { get; }
    }
}
