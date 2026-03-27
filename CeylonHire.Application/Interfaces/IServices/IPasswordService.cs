namespace CeylonHire.Application.Interfaces.IServices
{
    public interface IPasswordService
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
    }
}
