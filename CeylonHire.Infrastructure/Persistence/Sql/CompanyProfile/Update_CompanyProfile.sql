UPDATE [dbo].[CompanyProfiles]
   SET 
    [CompanyName] = @CompanyName,
    [Description] = @Description,
    [WebSite] = @WebSite,
    [LogoUrl] = @LogoUrl,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;