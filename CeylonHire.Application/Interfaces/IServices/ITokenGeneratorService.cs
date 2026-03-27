using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Interfaces.IServices
{
    public interface ITokenGeneratorService
    {
        /// <summary>
        /// generate jwt token for the user
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <returns>Returns a JWT token as a string.</returns>
        string GenerateToken(User user);
    }
}
