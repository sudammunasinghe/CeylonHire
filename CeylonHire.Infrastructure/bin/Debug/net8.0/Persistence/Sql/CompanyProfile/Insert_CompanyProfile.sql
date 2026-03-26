INSERT INTO [dbo].[CompanyProfiles]
(
	[UserId],
    [CompanyName],
    [Description],
    [WebSite],
    [LogoUrl]
)
VALUES
(
	@UserId,
	@CompanyName,
	@Description,
	@WebSite,
	@LogoUrl
);

SELECT CAST(SCOPE_IDENTITY() AS INT);

