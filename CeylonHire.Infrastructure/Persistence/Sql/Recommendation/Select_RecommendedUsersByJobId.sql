SELECT
	USK.[UserId],
	COUNT(*) [MatchCount]
FROM [dbo].[JobSkills] JSK
	INNER JOIN [dbo].[UserSkills] USK ON JSK.[SkillId] = USK.[SkillId] AND USK.[IsActive] = 1
WHERE JSK.[IsActive] = 1 AND JSK.[JobId] = @JobId
GROUP BY USK.[UserId]
ORDER BY [MatchCount] DESC;