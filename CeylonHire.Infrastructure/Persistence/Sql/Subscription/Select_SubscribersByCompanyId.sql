SELECT 
	JP.[UserId]
FROM [dbo].[CompanySubscription] CS
	INNER JOIN [dbo].[JobSeekerProfiles] JP ON CS.[JobseekerId] = JP.[Id] AND JP.[IsActive] = 1
WHERE CS.[CompanyId] = @CompanyId AND
	CS.[IsActive] = 1;