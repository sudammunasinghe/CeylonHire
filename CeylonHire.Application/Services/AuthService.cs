using CeylonHire.Api.Models;
using CeylonHire.Api.Models.JobSeeker;
using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;

namespace CeylonHire.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public AuthService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<string> RegisterNewJobseekerAsync(JobSeekerProfileDto dto)
        {
            var user = 
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if(user != null)
                throw new Exception("Email already exists.");

            var jobseekerProfileInfo = JobSeekerProfile.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.NIC,
                dto.ExperienceYears,
                dto.CVUrl
            );

            var newUser = User.Create(
                dto.Email,
                dto.Password,
                2
            );
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password);
            var jobseekerProfileId =  await _userRepository.RegisterNewJobseekerAsync(newUser, jobseekerProfileInfo);
            if(jobseekerProfileId == null)
                return "Registration failed.";
            return "Registration successful.";
        }
    }
}
