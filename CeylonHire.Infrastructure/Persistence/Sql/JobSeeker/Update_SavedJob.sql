UPDATE [dbo].[SavedJobs]
SET
	[IsActive] = @IsActive,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @SavedJobId;