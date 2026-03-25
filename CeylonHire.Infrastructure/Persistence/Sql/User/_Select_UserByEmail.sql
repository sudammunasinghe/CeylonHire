SELECT 
	[Id],
    [Email],
    [PassswordHash],
    [RoleId],
    [PasswordResetToken],
    [PasswordResetTokenExpiry],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Users]
WHERE [Email] = @Email;