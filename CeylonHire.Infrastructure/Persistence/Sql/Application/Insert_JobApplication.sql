INSERT INTO [dbo].[Applications]
(
	[JobId],
    [UserId],
    [StatusId],
    [CVUrl],
    [CoverLetterUrl]
)
VALUES
(
	@JobId, 
    @UserId,
    @StatusId,
    @CVUrl,
    @CoverLetterUrl
);