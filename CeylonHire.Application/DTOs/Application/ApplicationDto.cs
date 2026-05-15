namespace CeylonHire.Application.DTOs.Application
{
    public class ApplicationDto
    {
        public int JobId { get; set; }
        public FileDto? CV { get; set; }
        public FileDto? CoverLetter { get; set; }
    }
}
