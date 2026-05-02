SELECT 
	[Id],
    [UserId],
    [FirstName],
    [LastName],
    [Gender],
    [Address],
    [NIC],
    [DateOfBirth],
    [ExperienceYears],
    [CVUrl],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[JobSeekerProfiles]
WHERE [Id] = @JobSeekerProfileId AND
	[IsActive] = 1;