using CeylonHire.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public int? Id { get; set; }
        public int? JobId { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public string? CVUrl { get; set; }
        public string? CoverLetterUrl { get; set; }
        private JobApplication() { }

        public static void ValidateFileType(string fileType)
        {
            var allowedExtensions = new[] { ".pdf", ".docx"};
            if (!allowedExtensions.Contains(fileType))
                throw new DomainException("Invalid file type.");
        }
        public static JobApplication create(
            int? jobId, 
            int? userId, 
            int? statusId, 
            string? cvFileName,
            string? coverLetterFileName
            )
        {
            var cvFileType = Path.GetExtension(cvFileName);
            ValidateFileType(cvFileType);

            if(coverLetterFileName != null)
            {
                var coverLetterFileTyoe = Path.GetFileName(coverLetterFileName);
                ValidateFileType(coverLetterFileTyoe);
            }
            return new JobApplication
            {
                JobId = jobId,
                UserId = userId,
                StatusId = statusId
            };
        }

    }
}
