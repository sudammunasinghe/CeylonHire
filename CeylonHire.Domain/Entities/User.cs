using CeylonHire.Api.Models;
using CeylonHire.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string? PasswordResetToken { get; set; }
        public string? asswordResetTokenExpiry { get; set; }

        private User() { }

        public static void ValidateEmail(string? email)
        {
            if(string.IsNullOrWhiteSpace(email))
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
