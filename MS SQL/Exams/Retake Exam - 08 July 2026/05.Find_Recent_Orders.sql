USE [TechStore]

GO

SELECT 
	[Id] AS [OrderId],
	FORMAT([OrderDate], 'MM-dd') AS [OrderDate],
	[CustomerId],
	[StoreId],
	[ProductId]
FROM [Orders]
WHERE [OrderDate] > '01-01-2025'
ORDER BY [OrderDate] DESC, [CustomerId], [StoreId], [ProductId] 