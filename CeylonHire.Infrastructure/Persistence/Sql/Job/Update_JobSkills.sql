UPDATE [dbo].[JobSkills]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [JobId] = @JobId AND
	[SkillId] IN @SkillIds;