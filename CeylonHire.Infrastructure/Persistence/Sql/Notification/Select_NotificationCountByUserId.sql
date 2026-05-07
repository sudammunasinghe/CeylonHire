SELECT
	COUNT(*)
FROM [dbo].[NotificationRecipient]
WHERE [RecipientUserId] = @UserId AND
	[IsRead] = 0 AND [IsActive] = 1;