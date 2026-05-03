using CeylonHire.Domain.Enums;
using CeylonHire.Domain.Exceptions;

namespace CeylonHire.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public int? Id { get; set; }
        public int? JobId { get; set; }
        public int? UserId { get; set; }
        public ApplicationStatusEnum? Status { get; set; }
        public string? CVUrl { get; set; }
        public string? CoverLetterUrl { get; set; }
        private JobApplication() { }

        public static void ValidateFileType(string fileType)
        {
            var allowedExtensions = new[] { ".pdf", ".docx" };
            if (!allowedExtensions.Contains(fileType))
                throw new DomainException("Invalid file type.");
        }
        public static JobApplication create(
            int? jobId,
            int? userId,
            ApplicationStatusEnum? statusId,
            string? cvFileName,
            string? coverLetterFileName
            )
        {
            var cvFileType = Path.GetExtension(cvFileName);
            ValidateFileType(cvFileType);

            if (coverLetterFileName != null)
            {
                var coverLetterFileType = Path.GetExtension(coverLetterFileName);
                ValidateFileType(coverLetterFileType);
            }
            return new JobApplication
            {
                JobId = jobId,
                UserId = userId,
                Status = statusId
            };
        }

        public void ChangeStaus(ApplicationStatusEnum newStaus)
        {
            if (!CanTransitionTo(newStaus))
                throw new DomainException($"Invlaid status transition : {Status} -> {newStaus}");
            Status = newStaus;
        }
        private bool CanTransitionTo(ApplicationStatusEnum newStatus)
        {
            return Status switch
            {
                ApplicationStatusEnum.Applied => newStatus is ApplicationStatusEnum.UnderReview or ApplicationStatusEnum.Rejected,
                ApplicationStatusEnum.UnderReview => newStatus is ApplicationStatusEnum.Shortlisted or ApplicationStatusEnum.Rejected,
                ApplicationStatusEnum.Shortlisted => newStatus is ApplicationStatusEnum.Interviewing,
                ApplicationStatusEnum.Interviewing => newStatus is ApplicationStatusEnum.Hired or ApplicationStatusEnum.Rejected,
                _ => false
            };
        }

    }
}
