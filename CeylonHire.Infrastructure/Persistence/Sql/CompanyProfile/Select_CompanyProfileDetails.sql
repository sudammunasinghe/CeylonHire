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
WHERE (@UserId IS NULL OR [UserId] = @UserId) AND
	(@CompanyId IS NULL OR [Id] = @CompanyId)