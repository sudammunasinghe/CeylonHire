UPDATE [dbo].[UserSkills]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [SkillId] IN(@SkillIds) AND
	[UserId] = @UserId AND
	[IsActive] = 1;