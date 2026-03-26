namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IPasswordService
    {
        string HashPassword(string? password);
    }
}
