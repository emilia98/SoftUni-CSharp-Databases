USE [LibraryDb];

-- Take ALL Libraries, including those without books -> LEFT JOIN
-- First find all libraries that have genre mystery
WITH [MysteryBooksLibraryIdsCte]
AS
(
	SELECT
		[l].[Id]
		FROM [Libraries] [l]
		INNER JOIN [LibrariesBooks] [lb]
		ON [l].[Id] = [lb].[LibraryId]
		INNER JOIN [Books] [b]
		ON [lb].[BookId] = [b].[Id]
		INNER JOIN [Genres] [g]
		ON [b].[GenreId] = [g].[Id]
		WHERE [g].[Name] = 'Mystery'
)

-- Filter all libraries whose Id is NOT in Ids of Libraries with Mystery books
SELECT 
	[l].[Name] AS [Library],
	[c].[Email]
FROM [Libraries] [l]
INNER JOIN [Contacts] [c]
ON [l].[ContactId] = [c].[Id]
WHERE [l].[Id] NOT IN (
	SELECT [Id] FROM [MysteryBooksLibraryIdsCte]
)
ORDER BY [l].[Name]