USE [TechStore]

GO

SELECT 
	[s].[Name] AS [StoreName],
	COUNT([p].[Price]) AS [ProductCount],
	FORMAT(AVG([p].[Price]), 'N2') AS [AveragePrice]
FROM [Products] AS [p]
INNER JOIN [StoresProducts] AS [sp]
ON [sp].[ProductId] = [p].[Id]
INNER JOIN [Stores] AS [s]
ON [sp].[StoreId] = [s].[Id]
WHERE [p].[Price] >= 800
GROUP BY [s].[Id], [s].[Name]
HAVING COUNT([p].[Price]) >= 4
ORDER BY [AveragePrice] DESC