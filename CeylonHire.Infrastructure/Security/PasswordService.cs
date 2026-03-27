using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Infrastructure.Security
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Hashes the provided password using a secure hashing algorithm.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>Returns the hashed password.</returns>
        public string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies if the provided password matches the hashed password.
        /// </summary>
        /// <param name="password">The password to be verified.</param>
        /// <param name="hashPassword">The hashed password to compare with.</param>
        /// <returns>Returns true if the password matches the hashed password, otherwise false.</returns>
        public bool VerifyPassword(string? password, string? hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
