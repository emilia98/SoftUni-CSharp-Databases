USE [Accounting]

GO

-- Select all clients with VAT starting with 'IT'
SELECT *
	FROM [Clients]
	WHERE [NumberVAT] LIKE 'IT%'

-- Select all ProductClients, based on clients with VAT starting with 'IT'
SELECT *
	FROM [ProductsClients]
	WHERE [ClientId] IN (	
		SELECT [Id]
		FROM [Clients]
		WHERE [NumberVAT] LIKE 'IT%'
	)

-- Select all Invoices, based on clients with VAT starting with 'IT'
SELECT *
	FROM [Invoices]
	WHERE [ClientId] IN (
		SELECT [Id]
			FROM [Clients]
			WHERE [NumberVAT] LIKE 'IT%'
	)

DELETE
	FROM [ProductsClients]
	WHERE [ClientId] IN (	
		SELECT [Id]
		FROM [Clients]
		WHERE [NumberVAT] LIKE 'IT%'
	)

GO

DELETE
	FROM [Invoices]
	WHERE [ClientId] IN (
		SELECT [Id]
			FROM [Clients]
			WHERE [NumberVAT] LIKE 'IT%'
	)

GO

DELETE
	FROM [Clients]
	WHERE [NumberVAT] LIKE 'IT%'