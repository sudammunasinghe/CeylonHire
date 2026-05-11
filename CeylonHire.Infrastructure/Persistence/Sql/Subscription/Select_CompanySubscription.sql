SELECT 
	[Id],
    [JobseekerId],
    [CompanyId],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[CompanySubscription]
WHERE [JobseekerId] = @JobSeekerId AND
	[CompanyId] = @CompanyId;