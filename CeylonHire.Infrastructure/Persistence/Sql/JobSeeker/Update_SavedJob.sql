UPDATE [dbo].[SavedJobs]
SET
	[IsActive] = 1,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @SavedJobId;