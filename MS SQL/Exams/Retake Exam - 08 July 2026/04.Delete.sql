USE [TechStore]

GO

-- Select statement for customers, whose email is NULL
SELECT *
	FROM [Customers]
	WHERE [Email] IS NULL

-- Select statement for orders, which has clients with no provided email 
SELECT *
	FROM [Orders] AS [o]
	INNER JOIN [Customers] AS [c]
	ON [o].[CustomerId] = [c].[Id]
	WHERE [c].[Email] IS NULL

-- Delete statement for ORDERS
DELETE
	FROM [Orders]
	WHERE [Id] IN (
		SELECT [o].[Id]
			FROM [Orders] AS [o]
			INNER JOIN [Customers] AS [c]
			ON [o].[CustomerId] = [c].[Id]
			WHERE [c].[Email] IS NULL
	)

-- Delete statement for CUSTOMERS
DELETE
	FROM [Customers]
	WHERE [Email] IS NULL