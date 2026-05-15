SELECT 
	CP.[Id],
    CP.[UserId],
    CP.[CompanyName],
    CP.[Description],
    CP.[WebSite],
    CP.[LogoUrl],
    CP.[IsActive],
    CP.[CreatedDateTime],
    CP.[LastModifiedDateTime]
FROM [dbo].[CompanyProfiles] CP
	INNER JOIN [dbo].[Jobs] JB ON CP.[Id] = JB.[CompanyId] AND JB.[IsActive] = 1
WHERE JB.[Id] = @JobId AND
	CP.[IsActive] = 1;