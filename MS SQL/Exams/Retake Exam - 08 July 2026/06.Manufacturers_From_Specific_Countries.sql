USE [TechStore]

GO

SELECT
	[Name] AS [Manufacturer],
	[Country]
FROM [Manufacturers]
WHERE [Country] LIKE 'S%'
ORDER BY [Country], [Name]