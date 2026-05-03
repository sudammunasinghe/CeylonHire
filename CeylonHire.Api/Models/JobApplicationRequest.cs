namespace CeylonHire.Application.DTOs.Application
{
    public class JobApplicationRequest
    {
        public int JobId { get; set; }
        public IFormFile? CVFile { get; set; }
        public IFormFile? CoverLetterFile { get; set; }
    }
}
