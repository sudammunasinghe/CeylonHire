using CeylonHire.Domain.Exceptions;
namespace CeylonHire.Domain.Entities
{
    public class JobSeekerProfile : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public char? Gender { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ExperienceYears { get; set; }
        public string? CVUrl { get; set; }
        private JobSeekerProfile() { }

        public static void ValidateFirstName(string? firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new DomainException("First name is required.");
        }

        public static void ValidateLastName(string? lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
                throw new DomainException("Last name is required.");
        }

        public static NicInfo ExtractNicInfo(string? nic)
        {
            if (string.IsNullOrWhiteSpace(nic))
                throw new DomainException("NIC is required.");

            int year;
            int dayOfYear;
            char gender;

            if (nic.Length == 10)
            {
                year = 1900 + int.Parse(nic.Substring(0, 2));
                dayOfYear = int.Parse(nic.Substring(2, 3));
            }
            else if (nic.Length == 12)
            {
                year = int.Parse(nic.Substring(0, 4));
                dayOfYear = int.Parse(nic.Substring(4, 3));
            }
            else
            {
                throw new DomainException("Invalid NIC format.");
            }

            if (dayOfYear > 500)
            {
                gender = 'F';
                dayOfYear -= 500;
            }
            else
            {
                gender = 'M';
            }
            var dateOfBirth = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
            return new NicInfo
            {
                Gender = gender,
                DateOfBirth = dateOfBirth
            };
        }

        public static JobSeekerProfile Create(
            string? firstName,
            string? lastName,
            string? address,
            string? nic,
            int? experienceYears,
            string? cvUrl
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            var nicInfo = ExtractNicInfo(nic);

            return new JobSeekerProfile
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = nicInfo.Gender,
                Address = address,
                NIC = nic,
                DateOfBirth = nicInfo.DateOfBirth,
                ExperienceYears = experienceYears,
                CVUrl = cvUrl
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? address,
            string? nic,
            int? experienceYears,
            string? cvUrl
            )
        {
            NicInfo? nicInfo = null;
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                ValidateFirstName(firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                ValidateFirstName(lastName);
            }

            if (!string.IsNullOrWhiteSpace(nic))
            {
                nicInfo = ExtractNicInfo(nic);
            }

            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            Gender = nicInfo?.Gender ?? Gender;
            Address = address ?? Address;
            NIC = nic ?? NIC;
            DateOfBirth = nicInfo?.DateOfBirth ?? DateOfBirth;
            ExperienceYears = experienceYears ?? ExperienceYears;
            CVUrl = cvUrl ?? CVUrl;
        }
    }
}
