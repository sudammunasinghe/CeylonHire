SELECT
	NTF.[Id] [NotificationId],
	NTF.[Title],
	CASE
		WHEN US.[RoleId] = 2 THEN CONCAT(JSP.[FirstName],' ', JSP.[LastName],' ',NTF.[Message])
		WHEN US.[RoleId] = 3 THEN CONCAT(CP.[CompanyName],' ',NTF.[Message])
		ELSE 'Unknown'
	END AS [Message],
	NTT.[Description] [NotificationType],
	CASE 
		WHEN US.[RoleId] = 2 THEN CONCAT(JSP.[FirstName],' ', JSP.[LastName])
		WHEN US.[RoleId] = 3 THEN CP.[CompanyName]
		ELSE 'System'
	END AS [SentUser],
	NTF.[ActionUrl],
	NTR.[IsRead]
FROM [dbo].[Notification] NTF
	INNER JOIN [dbo].[NotificationRecipient] NTR ON NTF.[Id] = NTR.[NotificationId] AND NTR.[IsActive] = 1
	INNER JOIN [dbo].[NotificationType] NTT ON NTF.[NotificationTypeId] = NTT.[Id] AND NTT.[IsActive] = 1
	INNER JOIN [dbo].[Users] US ON NTF.[SentUserId] = US.[Id] AND US.[IsActive] = 1
	LEFT JOIN [dbo].[JobSeekerProfiles] JSP ON US.[Id] = JSP.[UserId] AND US.[RoleId] = 2 AND US.[IsActive] = 1
	LEFT JOIN [dbo].[CompanyProfiles] CP ON US.[Id] = CP.[UserId] AND US.[RoleId] = 3 AND CP.[IsActive] = 1
WHERE NTR.[RecipientUserId] = @UserId
ORDER BY NTR.[IsRead] ASC, NTR.[CreatedDateTime] DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT
	COUNT(*)
FROM [dbo].[NotificationRecipient] 
WHERE [RecipientUserId] = @UserId AND
	[IsActive] = 1;
