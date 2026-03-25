SELECT 
	[Id],
    [Email],
    [PasswordHash],
    [RoleId],
    [PasswordResetToken],
    [PasswordResetTokenExpiry],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Users]
WHERE [Email] = @Email;