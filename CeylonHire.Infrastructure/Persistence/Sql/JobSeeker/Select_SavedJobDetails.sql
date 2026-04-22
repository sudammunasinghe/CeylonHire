SELECT
	JB.[Id],
	JB.[Title],
	CP.[CompanyName],
	JB.[Location],
	JBM.[Description] [JobMode],
	JB.[CreatedDateTime] [PostedDate]
FROM [dbo].[Jobs] JB
	INNER JOIN [dbo].[CompanyProfiles] CP ON JB.[CompanyId] = CP.[Id] AND CP.[IsActive] = 1
	INNER JOIN [dbo].[SavedJobs] SJ ON JB.[Id] = SJ.[JobId] AND JB.[IsActive] = 1
	INNER JOIN [dbo].[JobModes] JBM ON JB.[JobModeId] = JBM.[Id] 
WHERE SJ.[JobSeekerId] = @JobSeekerId AND SJ.[IsActive] = 1;