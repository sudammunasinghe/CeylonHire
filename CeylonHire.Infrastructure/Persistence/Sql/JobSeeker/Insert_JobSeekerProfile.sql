INSERT INTO [dbo].[JobSeekerProfiles]
(
	[UserId],
	[FirstName],
	[LastName],
	[Gender],
	[Address],
	[NIC],
	[DateOfBirth],
	[ExperienceYears],
	[CVUrl]
)
VALUES(
	@UserId,
	@FirstName,
	@LastName,
	@Gender,
	@Address,
	@NIC,
	@DateOfBirth,
	@ExperienceYears,
	@CVUrl
);

SELECT CAST(SCOPE_IDENTITY() AS INT);