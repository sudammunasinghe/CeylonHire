UPDATE [dbo].[JobSeekerProfiles]
   SET 
    [FirstName] = @FirstName,
    [LastName] = @LastName,
    [Gender] = @Gender,
    [Address] = @Address,
    [NIC] = @NIC,
    [DateOfBirth] = @DateOfBirth,
    [ExperienceYears] = @ExperienceYears,
    [CVUrl] = @CVUrl,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;