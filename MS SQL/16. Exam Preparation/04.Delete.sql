USE [LibraryDb]

GO

-- Query to find Ids of Authors with Name 'Alex Michaelides'
-- Names are NOT unique, hence returned result will be 1 column x N rows (vector)
SELECT [Id]
	FROM [Authors]
	WHERE [Name] = 'Alex Michaelides'

-- Query to find Ids of Books of Author with Name 'Alex Michaelides'
-- Returned result is 1 column x N rows (vector)
SELECT [b].[Id]
			FROM [Books] AS [b]
			INNER JOIN [Authors] AS [a]
			ON [b].[AuthorId] = [a].[Id]
			WHERE [a].[Name] = 'Alex Michaelides'

-- Delete all rows in mapping table LibrariesBooks of Books to delete
DELETE
	FROM [LibrariesBooks]
	WHERE [BookId] IN (
		SELECT [b].[Id]
			FROM [Books] AS [b]
			INNER JOIN [Authors] AS [a]
			ON [b].[AuthorId] = [a].[Id]
			WHERE [a].[Name] = 'Alex Michaelides'
	)

-- Delete Books by 'Alex Michaelides'
DELETE
	FROM [Books]
	WHERE [AuthorId] IN (
	    SELECT [Id]
			FROM [Authors]
			WHERE [Name] = 'Alex Michaelides'
	)

-- Delete Author 'Alex Michaelides'
DELETE
	FROM [Authors]
	WHERE [Name] = 'Alex Michaelides'