UPDATE [dbo].[Jobs]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @JobId;