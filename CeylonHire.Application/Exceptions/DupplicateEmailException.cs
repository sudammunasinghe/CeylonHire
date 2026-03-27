namespace CeylonHire.Application.Exceptions
{
    public class DupplicateEmailException : Exception
    {
        public DupplicateEmailException(string message) : base(message) { }
    }
}
