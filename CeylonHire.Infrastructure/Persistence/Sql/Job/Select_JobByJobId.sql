SELECT 
	[Id],
    [CompanyId],
    [Title],
    [Description],
    [Salary],
    [Location],
    [NumberOfOpenings],
    [MinExperienceYears],
    [JobTypeId],
    [JobModeId],
    [ExperienceLevelId],
    [DeadLine],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Jobs]
WHERE [Id] = @JobId;