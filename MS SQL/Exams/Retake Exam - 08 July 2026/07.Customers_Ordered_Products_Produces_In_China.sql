USE [TechStore]

GO

SELECT 
	[c].[Name] AS [CustomerName],
	[c].[PhoneNumber],
	[c].[Email]
FROM [Customers] AS [c]
INNER JOIN [Orders] AS [o]
ON [o].[CustomerId] = [c].[Id]
INNER JOIN [Products] AS [p]
ON [o].[ProductId] = [p].[Id]
INNER JOIN [Manufacturers] AS [m]
ON [p].[ManufacturerId] = [m].[Id]
WHERE [c].[Email] IS NOT NULL AND [m].[Country] = 'China'
GROUP BY [o].[ProductId], [c].[Name], [c].[PhoneNumber], [c].[Email]
ORDER BY [c].[Name]