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
    @Status,
    @CVUrl,
    @CoverLetterUrl
);

SELECT CAST(SCOPE_IDENTITY() AS INT);