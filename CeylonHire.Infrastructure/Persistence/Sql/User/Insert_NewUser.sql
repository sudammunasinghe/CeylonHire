INSERT INTO [dbo].[Users]
(
	[Email],
	[PasswordHash],
	[RoleId]
)
VALUES(
	@Email,
	@PasswordHash,
	@RoleId
)

SELECT CAST(SCOPE_IDENTITY() AS INT);