INSERT INTO [dbo].[Jobs]
(
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
    [DeadLine]
)
VALUES(
	@CompanyId,
	@Title,
	@Description,
	@Salary,
	@Location,
	@NumberOfOpenings,
	@MinExperienceYears,
	@JobTypeId,
	@JobModeId,
	@ExperienceLevelId,
	@DeadLine
);

SELECT CAST(SCOPE_IDENTITY() AS INT);



