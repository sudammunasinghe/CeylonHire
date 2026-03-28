UPDATE [dbo].[Users]
SET
	[PasswordResetToken] = @Token,
	[PasswordResetTokenExpiry] = @Expiry,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId;