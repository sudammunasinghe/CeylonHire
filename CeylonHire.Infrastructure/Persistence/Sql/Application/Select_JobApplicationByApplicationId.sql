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
WHERE [Id] = @ApplicationId AND
	[IsActive] = 1;