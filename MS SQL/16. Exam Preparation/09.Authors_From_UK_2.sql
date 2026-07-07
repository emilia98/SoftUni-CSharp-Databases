USE [LibraryDb]

SELECT	
	[a].[Name] AS [Author],
	[c].[Email],
	[c].[PostAddress] AS [Address]
FROM [Authors] [a]
LEFT JOIN [Contacts] [c]
ON [a].[ContactId] = [c].[Id]
WHERE CHARINDEX('UK', [c].[PostAddress]) > 0
ORDER BY [a].[Name]