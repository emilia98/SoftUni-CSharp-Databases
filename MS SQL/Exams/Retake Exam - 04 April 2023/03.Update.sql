USE [Accounting]

GO

-- Select statement to get the data to update
SELECT *
	FROM [Invoices]
	WHERE DATEPART(MONTH, [IssueDate]) = 11 AND DATEPART(YEAR, [IssueDate]) = 2022

UPDATE [Invoices]
	SET [DueDate] = '2023-04-01'
 	FROM [Invoices]
	WHERE DATEPART(MONTH, [IssueDate]) = 11 AND DATEPART(YEAR, [IssueDate]) = 2022

-- Select statement to get the data to update
SELECT *
	FROM [Clients] AS [c]
	INNER JOIN [Addresses] AS [a]
	ON [c].[AddressId] = [a].[Id]
	WHERE [c].[Name] LIKE '%CO%'

UPDATE [Clients]
	SET [AddressId] = 3
 	WHERE [Name] LIKE '%CO%'