using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(User user);
    }
}
