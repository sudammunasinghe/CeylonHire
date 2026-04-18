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
WHERE JB.[IsActive] = 1
	AND (@Search IS NULL OR JB.[Title] LIKE '%' + @Search + '%')
	AND (@Location IS NULL OR JB.[Location]  = @Location)
	AND (@JobTypeId IS NULL OR JB.[JobTypeId] = @JobTypeId)
	AND (@JobModeId IS NULL OR JB.[JobModeId] = @JobModeId)
ORDER BY JB.[Id]
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY

SELECT
	COUNT(*)
FROM [dbo].[Jobs] 
WHERE [IsActive] = 1
	AND (@Search IS NULL OR [Title] LIKE '%' + @Search + '%')
	AND (@Location IS NULL OR [Location]  = @Location)
	AND (@JobTypeId IS NULL OR [JobTypeId] = @JobTypeId)
	AND (@JobModeId IS NULL OR [JobModeId] = @JobModeId)
