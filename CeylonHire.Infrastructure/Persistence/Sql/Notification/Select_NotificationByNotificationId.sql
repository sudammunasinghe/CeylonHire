SELECT 
	[Id],
    [NotificationId],
    [RecipientUserId],
    [IsRead],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[NotificationRecipient]
WHERE [NotificationId] = @NotificationId AND
	[RecipientUserId] = @RecipientUserId AND [IsActive] = 1;