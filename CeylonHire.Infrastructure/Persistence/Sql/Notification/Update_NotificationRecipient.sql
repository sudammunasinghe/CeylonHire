UPDATE [dbo].[NotificationRecipient]
   SET 
    [IsRead] = @IsRead,
    [LastModifiedDateTime] = @LastModifiedDateTime
WHERE [Id] = @Id;