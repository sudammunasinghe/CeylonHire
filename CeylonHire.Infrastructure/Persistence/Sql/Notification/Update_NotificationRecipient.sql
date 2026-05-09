UPDATE [dbo].[NotificationRecipient]
   SET 
    [IsActive] = @IsActive,
    [IsRead] = @IsRead,
    [LastModifiedDateTime] = @LastModifiedDateTime
WHERE [Id] = @Id;