USE [LibraryDb]

SELECT 
	[b].[Id],
	[b].[Title],
	[b].[ISBN],
	[g].[Name] AS [Genre]
	FROM [Books] [b]
INNER JOIN [Genres] [g]
	ON [b].[GenreId] = [g].[Id]
	WHERE [g].[Name] IN ('Biography', 'Historical Fiction')
	ORDER BY [g].[Name] ASC, [b].[Title] ASC 