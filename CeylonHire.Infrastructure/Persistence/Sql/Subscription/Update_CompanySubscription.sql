UPDATE [dbo].[CompanySubscription]
   SET 
    [IsActive] = @IsActive,
    [LastModifiedDateTime] = @LastModifiedDateTime
WHERE [JobseekerId] = @JobseekerId AND
	[CompanyId] = @CompanyId;
