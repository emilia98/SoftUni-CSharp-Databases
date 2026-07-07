USE [LibraryDb]

UPDATE [Contacts]
	SET [Website] = CONCAT('www.', LOWER(REPLACE([a].[Name], ' ', '')) ,'.com')
	FROM [Contacts]
	AS [c]
INNER JOIN [Authors] [a]
ON [a].[ContactId] = [c].[Id]
WHERE [c].[Website] IS NULL