INSERT INTO [dbo].[ApplicationHistory]
(
	[ApplicationId],
    [StatusId],
    [ActionTriggeredUser],
    [Remark]
)
VALUES
(
	@ApplicationId,
    @Status,
    @ActionTriggeredUser,
    @Remark
);