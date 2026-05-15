INSERT INTO [dbo].[Notification]
(
	[Title],
    [Message],
    [SentUserId],
    [NotificationTypeId]
)
VALUES
(
	@Title, 
    @Message, 
    @SentUserId, 
    @NotificationTypeId
);
SELECT CAST(SCOPE_IDENTITY() AS INT);