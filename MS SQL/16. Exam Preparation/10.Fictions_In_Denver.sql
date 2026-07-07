USE [LibraryDb]

SELECT
	[a].[Name] AS [Author],
	[b].[Title],
	[l].[Name] AS [Library],
	[c].[PostAddress] AS [Library Address]
FROM [Books] [b]
INNER JOIN [Genres] [g]
ON [b].[GenreId] = [g].[Id]
INNER JOIN [LibrariesBooks] [lb]
ON [lb].[BookId] = [b].[Id]
INNER JOIN [Libraries] [l]
ON [lb].[LibraryId] = [l].[Id]
INNER JOIN [Contacts] [c]
ON [l].[ContactId] = [c].[Id]
INNER JOIN [Authors] [a]
ON [b].[AuthorId] = [a].[Id]
WHERE [g].[Name] = 'Fiction' AND [c].[PostAddress] LIKE '%Denver%'
ORDER BY [b].[Title]