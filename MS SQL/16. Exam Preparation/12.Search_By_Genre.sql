USE [LibraryDb]

GO

CREATE PROCEDURE usp_SearchBookByGenre(@genreName NVARCHAR(30), @city NVARCHAR(100) = NULL)
	AS
 BEGIN
		    SELECT [b].[Title],
				   [b].[YearPublished]
				AS [Year],
				   [b].[ISBN],
				   [a].[Name]
				AS [Author],
				   [g].[Name]
				AS [Genre]
			  FROM [Books] AS [b]
		INNER JOIN [Genres]
			    AS [g]
				ON [b].[GenreId] = [g].[Id]
		INNER JOIN [Authors] AS [a]
				ON [b].[AuthorId] = [a].[Id]
		 LEFT JOIN [LibrariesBooks] AS [lb]
				ON [b].[Id] = [lb].[BookId]
		 LEFT JOIN [Libraries] AS [l]
				ON [lb].[LibraryId] = [l].[Id]
		 LEFT JOIN [Contacts] AS [c]
				ON [l].[ContactId] = [c].[Id]
			 WHERE [g].[Name] LIKE @genreName 
			       AND 
				   (@city IS NULL OR CHARINDEX(@city, [c].[PostAddress]) > 0)
		  ORDER BY [b].[Title],
				   [b].[YearPublished] DESC
END


GO

EXEC usp_SearchBookByGenre 'Fantasy'
EXEC usp_SearchBookByGenre 'Fantasy', 'Denver'