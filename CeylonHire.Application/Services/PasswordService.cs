using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
