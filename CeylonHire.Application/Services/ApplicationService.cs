using CeylonHire.Application.DTOs.Application;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using CeylonHire.Domain.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace CeylonHire.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;
        public ApplicationService(
            IApplicationRepository applicationRepository, 
            ICurrentUserService currentUserService,
            INotificationService notificationService
            )
        {
            _applicationRepository = applicationRepository;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
        }

        public async Task ApplyJobAsync(ApplicationDto dto)
        {
            if (dto?.CV == null)
                throw new BadRequestException("CV is required.");

            var loggedUser = _currentUserService.UserId;
            var job =
                await _applicationRepository.GetJobByJobIdAsync(dto.JobId);

            if (job == null)
                throw new NotFoundException("Job not found.");

            var jobApplication =
                await _applicationRepository.GetJobApplicationAsync(loggedUser, job.Id);

            if (jobApplication != null)
                throw new ConflictException("You already applied this job.");

            var newApplication = JobApplication.create(
                job.Id,
                loggedUser,
                ApplicationStatusEnum.Applied,
                dto?.CV?.FileName,
                dto?.CoverLetter?.FileName
            );

            var cvSubFolder = "cv";
            string? coverLetterFullPath = null;
            string? coverLetterFileUrl = null;

            GenerateFilePathAndUrl(
                job.Id,
                cvSubFolder,
                dto.CV.FileName,
                out string cvFullPath,
                out string cvFileUrl
            );

            if (dto.CoverLetter != null)
            {
                var coverLetterSubFolder = "coverLetter";
                GenerateFilePathAndUrl(
                    job.Id,
                    coverLetterSubFolder,
                    dto.CoverLetter.FileName,
                    out coverLetterFullPath,
                    out coverLetterFileUrl
                );
            }

            var company =
                await _applicationRepository.GetCompanyByJobIdAsync(job.Id);

            var title = "Application Receieved";
            var message = $"Submitted application for job : {job.Title}";
            var notificationTypeId = 1;
            List<int> recipientUsers = new List<int> { company.UserId };

            try
            {
                await SaveFileAsync(cvFullPath, dto.CV.FileStream);
                newApplication.CVUrl = cvFileUrl;
                
                if (dto.CoverLetter != null)
                {
                    await SaveFileAsync(coverLetterFullPath, dto.CoverLetter.FileStream);
                    newApplication.CoverLetterUrl = coverLetterFileUrl;
                }
                await _applicationRepository.ApplyJobAsync(newApplication);
                await _notificationService.SendNotificationAsync(title, message, notificationTypeId, recipientUsers);
            }
            catch
            {
                if (File.Exists(cvFullPath))
                    File.Delete(cvFullPath);

                if (File.Exists(coverLetterFullPath))
                    File.Delete(coverLetterFullPath);
                throw;
            }
        }

        public async Task ManageJobApplicationAsync(int applicationId, ApplicationStatusEnum newStatus)
        {
            var loggedUser = _currentUserService.UserId;
            var title = "";
            var message = "";

            var application =
                await _applicationRepository.GetJobApplicationByApplicationIdAsync(applicationId);

            if (application == null)
                throw new NotFoundException("Application not found.");

            var job =
                await _applicationRepository.GetJobByJobIdAsync(application.JobId);

            if (job == null)
                throw new NotFoundException("Job not found.");

            var company =
                await _applicationRepository.GetCompanyByJobIdAsync(application.JobId);

            if (company == null)
                throw new NotFoundException("Company not found.");

            if (company.UserId != loggedUser)
                throw new UnauthorizedAccessException("Access denied.");

            switch (newStatus)
            {
                case ApplicationStatusEnum.UnderReview:
                    title = "Application Under Review";
                    message = $"Your application for the {job.Title} position at {company.CompanyName} is currently being reviewed.";
                    break;

                case ApplicationStatusEnum.Shortlisted:
                    title = "Application Shortlisted";
                    message = $"Congratulations! Your application for the {job.Title} position at {company.CompanyName} has been shortlisted.";
                    break;

                case ApplicationStatusEnum.Rejected:
                    title = "Application Update";
                    message = $"Thank you for your interest in the {job.Title} position at {company.CompanyName}. After careful consideration, we have decided not to proceed with your application at this time.";
                    break;

                case ApplicationStatusEnum.Hired:
                    title = "Interview Scheduled";
                    message = $"Congratulations! You have been selected for the {job.Title} position at {company.CompanyName}.";
                    break;

                case ApplicationStatusEnum.Interviewing:
                    title = "Interview Invitation";
                    message = $"Your interview for the {job.Title} position at {company.CompanyName} has been scheduled. Please check your email for the details.";
                    break;

                default:
                    throw new BadRequestException("Invalid application status.");
            }

            application.ChangeStaus(newStatus);
            application.LastModifiedDateTime = DateTime.Now;
            var notificationTypeId = 1;
            List<int> recipientUsers = new List<int> { (int)application.UserId };

            await _applicationRepository.ManageJobApplicationAsync(loggedUser, application);
            await _notificationService.SendNotificationAsync(
                title, 
                message, 
                notificationTypeId, 
                recipientUsers
            );

        }

        private void GenerateFilePathAndUrl(
            int jobId,
            string subFolder,
            string fileName,
            out string fullPath,
            out string fileUrl
            )
        {
            var folder = Path.Combine("wwwroot", subFolder, jobId.ToString());
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            fullPath = Path.Combine(folder, uniqueFileName);
            fileUrl = $"{subFolder}/{jobId}/{uniqueFileName}";
        }

        private async Task SaveFileAsync(string fullFilePath, Stream fileStream)
        {
            using var fs = new FileStream(fullFilePath, FileMode.Create);
            await fileStream.CopyToAsync(fs);
        }
    }
}
