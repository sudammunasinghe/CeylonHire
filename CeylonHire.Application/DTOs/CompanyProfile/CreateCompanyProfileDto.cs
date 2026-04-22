namespace CeylonHire.Application.DTOs.CompanyProfile
{
    public class CreateCompanyProfileDto : CompanyProfileDetails
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
