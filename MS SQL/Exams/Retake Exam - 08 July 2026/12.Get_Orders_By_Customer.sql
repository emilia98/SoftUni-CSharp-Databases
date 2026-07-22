USE [TechStore]

GO

SELECT 
	[p].[Name] AS [ProductName],
	[s].[Name] AS [StoreName],
	FORMAT([o].[OrderDate], 'MM-dd-yyyy') AS [OrderDate],
	FORMAT([p].[Price], 'N2') AS [Price]
FROM [Customers] AS [c]
INNER JOIN [Orders] AS [o]
ON [o].[CustomerId] = [c].[Id]
INNER JOIN [Products] AS [p]
ON [o].[ProductId] = [p].[Id]
INNER JOIN [Stores] AS [s]
ON [o].[StoreId] = [s].[Id]
WHERE [c].[Name] = 'Carlos Fernández'
ORDER BY [OrderDate] DESC, [ProductName]

GO

CREATE PROCEDURE usp_GetOrdersByCustomer(@customerName NVARCHAR(80))
AS
BEGIN
	SELECT 
		[p].[Name] AS [ProductName],
		[s].[Name] AS [StoreName],
		FORMAT([o].[OrderDate], 'MM-dd-yyyy') AS [OrderDate],
		FORMAT([p].[Price], 'N2') AS [Price]
	FROM [Customers] AS [c]
	INNER JOIN [Orders] AS [o]
	ON [o].[CustomerId] = [c].[Id]
	INNER JOIN [Products] AS [p]
	ON [o].[ProductId] = [p].[Id]
	INNER JOIN [Stores] AS [s]
	ON [o].[StoreId] = [s].[Id]
	WHERE [c].[Name] = @customerName
	ORDER BY [OrderDate] DESC, [ProductName]
END

GO

EXEC usp_GetOrdersByCustomer 'Carlos Fernández'