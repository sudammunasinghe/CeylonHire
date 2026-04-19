SELECT 
	JB.[Id],
    JB.[CompanyId],
    JB.[Title],
    JB.[Description],
    JB.[Salary],
    JB.[Location],
    JB.[NumberOfOpenings],
    JB.[MinExperienceYears],
    JBT.[Description] [JobType],
    JBM.[Description] [JobMode],
    EXL.[Description] [ExperienceLevel],
    JB.[DeadLine]
FROM [dbo].[Jobs] JB
	INNER JOIN [dbo].[JobTypes] JBT ON JB.[JobTypeId] = JBT.[Id]
	INNER JOIN [dbo].[JobModes] JBM ON JB.[JobModeId] = JBM.[Id]
	INNER JOIN [dbo].[ExperienceLevel] EXL ON JB.[ExperienceLevelId] = EXL.[Id]
WHERE JB.[IsActive] = 1 AND
    JB.[Id] = @JobId;


SELECT
	SK.[SkillName]
FROM [dbo].[Jobs] JB
	INNER JOIN [dbo].[JobSkills] JK ON JB.[Id] = JK.[JobId] AND JK.[IsActive] = 1
	INNER JOIN [dbo].[Skills] SK ON JK.[SkillId] = SK.[Id] AND SK.[IsActive] = 1
WHERE JB.[IsActive] = 1 AND
    JB.[Id] = @JobId;