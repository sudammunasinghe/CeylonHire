SELECT 
	[Id],
    [Email],
    [PasswordHash],
    [RoleId],
    [PasswordResetTokenId],
    [PasswordResetTokenHash],
    [PasswordResetTokenExpiry],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Users]
WHERE [Email] = @Email;