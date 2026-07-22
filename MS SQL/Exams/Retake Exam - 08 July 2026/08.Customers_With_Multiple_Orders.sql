USE [TechStore]

GO

SELECT 
	[c].[Name] AS [CustomerName],
	COUNT([o].[CustomerId]) AS [OrdersCount]
FROM [Customers] AS [c]
INNER JOIN [Orders] AS [o]
ON [o].[CustomerId] = [c].[Id]
GROUP BY [o].[CustomerId], [c].[Name]
HAVING COUNT([o].[CustomerId]) > 1
ORDER BY [OrdersCount] DESC, [CustomerName]