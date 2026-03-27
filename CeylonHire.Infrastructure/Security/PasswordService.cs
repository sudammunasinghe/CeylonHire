using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Infrastructure.Security
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string? password, string? hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
