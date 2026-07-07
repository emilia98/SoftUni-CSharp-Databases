USE [LibraryDb]

SELECT TOP(3)
	[b].[Title],
	[b].[YearPublished] AS [Year],
	[g].[Name] AS [Genre]
FROM [Books] [b]
INNER JOIN [Genres] [g]
ON [b].[GenreId] = [g].[Id]
WHERE ([b].[YearPublished] > 2000 AND [b].[Title] LIKE '%a%')
		OR ([b].[YearPublished] < 1950 AND [g].[Name] LIKE '%Fantasy%')
ORDER BY [b].[Title], [b].[YearPublished] DESC