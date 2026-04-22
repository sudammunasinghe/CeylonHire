SELECT 
	[Id],
    [UserId],
    [CompanyName],
    [Description],
    [WebSite],
    [LogoUrl],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[CompanyProfiles]
WHERE [UserId] = @UserId;