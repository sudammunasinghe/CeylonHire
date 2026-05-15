INSERT INTO [dbo].[NotificationRecipient]
(
	[NotificationId],
    [RecipientUserId]
)
VALUES
(
	@NotificationId,
    @RecipientUserId
);