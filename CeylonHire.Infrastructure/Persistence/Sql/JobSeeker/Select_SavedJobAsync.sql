SELECT 
	[Id],
    [JobSeekerId],
    [JobId],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[SavedJobs]
WHERE [JobSeekerId] = @JobSeekerId AND
	[JobId] = @JobId;