USE [LibraryDb]

SELECT	
	[a].[Name] AS [Author],
	[c].[Email],
	[c].[PostAddress] AS [Address]
FROM [Authors] [a]
INNER JOIN [Contacts] [c]
ON [a].[ContactId] = [c].[Id]
WHERE [c].[PostAddress] LIKE '%UK%'
ORDER BY [a].[Name]