SELECT 
	[Id],
    [NotificationId],
    [RecipientUserId],
    [IsRead],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[NotificationRecipient]
WHERE [RecipientUserId] = @RecipientUserId AND
	[IsRead] = 0 AND [IsActive] = 1;