namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IHashingService
    {
        /// <summary>
        /// hash the provided password using a secure hashing algorithm and return the hashed password.
        /// </summary>
        /// <param name="password">The password of the user.</param>
        /// <returns>Returns the hashed password.</returns>
        string HashPassword(string? password);

        /// <summary>
        /// Verifies if the provided password matches the hashed password.
        /// </summary>
        /// <param name="password">The password of the user.</param>
        /// <param name="hashedPassword">The hashed password to compare with.</param>
        /// <returns>Returns true if the password matches the hashed password, otherwise false.</returns>
        bool VerifyPassword(string? password, string? hashedPassword);

        /// <summary>
        /// hashes the provided password reset token using a secure hashing algorithm and return the hashed token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <returns>Returns the hashed password reset token.</returns>
        string HashPasswordResetToken(string? token);

        /// <summary>
        /// Verifies if the provided password reset token matches the hashed token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="hashToken">The hashed token to compare with.</param>
        /// <returns>Returns true if the token matches the hashed token, otherwise false.</returns>
        bool VerifyPasswordResetToken(string? token, string? hashToken);
    }
}
