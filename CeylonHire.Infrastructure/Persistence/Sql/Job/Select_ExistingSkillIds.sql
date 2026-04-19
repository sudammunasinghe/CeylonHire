SELECT 
	[SkillId]
FROM [dbo].[JobSkills]
WHERE [JobId] = @JobId AND
	[IsActive] = 1;