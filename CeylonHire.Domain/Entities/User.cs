using CeylonHire.Domain.Exceptions;

namespace CeylonHire.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
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

        public static void ValidatePassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required.");
        }

        public static User Create(
            string email,
            string password,
            int roleId
            )
        {
            ValidateEmail(email);
            ValidatePassword(password);

            return new User
            {
                Email = email,
                RoleId = roleId
            };
        }
    }
}
