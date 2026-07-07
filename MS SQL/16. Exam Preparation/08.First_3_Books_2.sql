USE [LibraryDb]

SELECT TOP(3)
	[b].[Title],
	[b].[YearPublished] AS [Year],
	[g].[Name] AS [Genre]
FROM [Books] [b]
INNER JOIN [Genres] [g]
ON [b].[GenreId] = [g].[Id]
WHERE ([b].[YearPublished] > 2000 AND CHARINDEX('a', [b].[Title]) > 0)
		OR ([b].[YearPublished] < 1950 AND CHARINDEX('Fantasy', [g].[Name]) > 0)
ORDER BY [b].[Title], [b].[YearPublished] DESC