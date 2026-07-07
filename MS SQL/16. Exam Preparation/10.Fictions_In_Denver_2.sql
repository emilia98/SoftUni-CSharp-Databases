USE [LibraryDb]

SELECT
	[a].[Name] AS [Author],
	[b].[Title],
	[l].[Name] AS [Library],
	[c].[PostAddress] AS [Library Address]
FROM [LibrariesBooks] [lb]
INNER JOIN [Books] [b]
ON [lb].[BookId] = [b].[Id]
INNER JOIN [Genres] [g]
ON [b].[GenreId] = [g].[Id]
INNER JOIN [Authors] [a]
ON [b].[AuthorId] = [a].[Id]
INNER JOIN [Libraries] [l]
ON [lb].[LibraryId] = [l].[Id]
INNER JOIN [Contacts] [c]
ON [l].[ContactId] = [c].[Id]
WHERE [g].[Name] = 'Fiction' AND [c].[PostAddress] LIKE '%Denver%'
ORDER BY [b].[Title]