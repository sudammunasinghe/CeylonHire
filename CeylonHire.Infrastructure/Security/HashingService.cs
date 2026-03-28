using CeylonHire.Application.Interfaces.IServices;

namespace CeylonHire.Infrastructure.Security
{
    public class HashingService : IHashingService
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

        /// <summary>
        /// hashes the provided password reset token using a secure hashing algorithm and return the hashed token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <returns>Returns the hashed password reset token.</returns>
        public string HashPasswordResetToken(string? token)
        {
            return BCrypt.Net.BCrypt.HashPassword(token);
        }

        /// <summary>
        /// verifies if the provided password reset token matches the hashed token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="hashToken">The hashed token to compare with.</param>
        /// <returns>Returns true if the token matches the hashed token, otherwise false.</returns>
        public bool VerifyPasswordResetToken(string? token, string? hashToken)
        {
            return BCrypt.Net.BCrypt.Verify(token, hashToken);
        }
    }
}
