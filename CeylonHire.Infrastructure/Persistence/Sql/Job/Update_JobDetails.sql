UPDATE [dbo].[Jobs]
   SET 
	[CompanyId] = @CompanyId,
    [Title] = @Title,
    [Description] = @Description,
    [Salary] = @Salary,
    [Location] = @Location,
    [NumberOfOpenings] = @NumberOfOpenings,
    [MinExperienceYears] = @MinExperienceYears,
    [JobTypeId] = @JobTypeId,
    [JobModeId] = @JobModeId,
    [ExperienceLevelId] = @ExperienceLevelId,
    [DeadLine] = @DeadLine,
    [LastModifiedDateTime] = GETDATE()
 WHERE [Id] = @Id;