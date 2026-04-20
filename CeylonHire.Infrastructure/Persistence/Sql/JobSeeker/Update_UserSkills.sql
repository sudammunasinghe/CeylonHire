UPDATE [dbo].[UserSkills]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] IN(@SkillIds);