UPDATE [dbo].[Applications]
   SET 
	[JobId] = @JobId,
    [UserId] = @UserId,
    [StatusId] = @Status,
    [CVUrl] = @CVUrl,
    [CoverLetterUrl] = @CoverLetterUrl,
    [IsActive] = @IsActive,
    [CreatedDateTime] = @CreatedDateTime,
    [LastModifiedDateTime] = @LastModifiedDateTime
WHERE [Id] = @Id;