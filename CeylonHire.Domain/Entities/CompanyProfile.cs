using CeylonHire.Domain.Exceptions;

namespace CeylonHire.Domain.Entities
{
    public class CompanyProfile : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public string? WebSite { get; set; }
        public string? LogoUrl { get; set; }
        private CompanyProfile() { }

        public static void ValidateCompanyName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Compnay name is required.");
        }

        public static CompanyProfile Create(
            string? companyName,
            string? description,
            string? webSite,
            string? logoUrl
            )
        {
            ValidateCompanyName(companyName);
            return new CompanyProfile
            {
                CompanyName = companyName,
                Description = description,
                WebSite = webSite,
                LogoUrl = logoUrl
            };
        }

        public void Update(
            string? companyName,
            string? description,
            string? webSite,
            string? logoUrl
            )
        {
            if(!string.IsNullOrWhiteSpace(companyName))
                ValidateCompanyName(companyName);

            CompanyName = companyName ?? CompanyName;
            Description = description ?? Description;
            WebSite = webSite ?? WebSite;
            LogoUrl = logoUrl ?? LogoUrl;
        }
    }
}
