UPDATE [dbo].[CompanyProfiles]
   SET 
	[UserId] = @UserId,
    [CompanyName] = @CompanyName,
    [Description] = @Description,
    [WebSite] = @WebSite,
    [LogoUrl] = @LogoUrl,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;