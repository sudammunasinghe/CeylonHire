using CeylonHire.Domain.Exceptions;

namespace CeylonHire.Domain.ValueObjects
{
    public class Password
    {
        public string Value { get; }
        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Password can not be empty.");

            if (!IsValid(value))
                throw new DomainException("Password does not meet strength requirements.");
            Value = value;
        }

        private bool IsValid(string password)
        {
            return (password.Length > 8 &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit)
            );
        }
    }
}
