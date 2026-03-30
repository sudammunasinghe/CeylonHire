UPDATE [dbo].[Users]
SET
	[PasswordHash] = @PasswordHash,
	[PasswordResetTokenId] = NULL,
	[PasswordResetTokenHash] = NULL,
	[PasswordResetTokenExpiry] = NULL,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId;