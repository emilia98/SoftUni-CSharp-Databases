USE [LibraryDb]

GO

CREATE FUNCTION [udf_AuthorsWithBooks](@name NVARCHAR(100))
    RETURNS INT
             AS
          BEGIN
			RETURN(
					SELECT COUNT([b].[Id])
						AS [BooksCount]
					  FROM [LibrariesBooks]
						AS [lb]
				INNER JOIN [Books]
						AS [b]
						ON [lb].[BookId] = [b].[Id]
				INNER JOIN [Authors]
     					AS [a]
						ON [b].[AuthorId] = [a].[Id]
					 WHERE [a].[Name] = @name
			)	
            END

GO

SELECT dbo.udf_AuthorsWithBooks('J.K. Rowling')