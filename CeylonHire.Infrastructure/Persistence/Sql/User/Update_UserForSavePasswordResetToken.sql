UPDATE [dbo].[Users]
SET
	[PasswordResetTokenId] = @TokenId,
	[PasswordResetTokenHash] = @TokenHash,
	[PasswordResetTokenExpiry] = @Expiry,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId;