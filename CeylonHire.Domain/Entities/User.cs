using CeylonHire.Domain.Exceptions;

namespace CeylonHire.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; private set; }
        public int RoleId { get; set; }
        public Guid? PasswordResetTokenId { get; set; }
        public string? PasswordResetTokenHash { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        private User() { }

        public static void ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required.");
        }

        public static User Create(
            string email,
            string passwordHash,
            int roleId
            )
        {
            ValidateEmail(email);

            var user = new User
            {
                Email = email,
                RoleId = roleId
            };
            user.ChangePassword(passwordHash);
            return user;
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
