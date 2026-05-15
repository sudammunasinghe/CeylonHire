SELECT 
	[Id],
    [JobId],
    [UserId],
    [StatusId] [Status],
    [CVUrl],
    [CoverLetterUrl],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Applications]
WHERE [UserId] = @UserId AND
	[JobId] = @JobId AND [IsActive] = 1;