using CeylonHire.Application.DTOs.Application;
using CeylonHire.Application.Exceptions;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICurrentUserService _currentUserService;
        public ApplicationService(IApplicationRepository applicationRepository, ICurrentUserService currentUserService)
        {
            _applicationRepository = applicationRepository;
            _currentUserService = currentUserService;
        }

        public async Task ApplyJobAsync(ApplicationDto dto)
        {
            if (dto?.CV == null)
                throw new BadRequestException("CV is required.");

            var loggedUser =_currentUserService.UserId;
            if (loggedUser == null)
                throw new UnauthorizedAccessException("Unauthorized");

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
                1,
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
            }
            catch
            {
                if(File.Exists(cvFullPath))
                    File.Delete(cvFullPath);

                if(File.Exists(coverLetterFullPath))
                    File.Delete(coverLetterFullPath);
                throw;
            }
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
            if(!Directory.Exists(folder))
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
