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
WHERE [UserId] = @UserId AND
	[IsActive] = 1;

SELECT
	UK.[Id],
	SK.[SkillName]
FROM [dbo].[UserSkills] UK
	INNER JOIN [dbo].[Skills] SK ON UK.[SkillId] = SK.[Id] AND SK.[IsActive] = 1
WHERE UK.[UserId] = @UserId AND 
	UK.[IsActive] = 1;


