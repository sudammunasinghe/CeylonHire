SELECT
	[SkillId]
FROM [dbo].[UserSkills]
WHERE [UserId] = @UserId AND
	[IsActive] = 1;