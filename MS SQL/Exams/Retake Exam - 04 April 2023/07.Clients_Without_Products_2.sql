USE [Accounting]

GO

SELECT 
	[c].[Id],
	[c].[Name] AS [Client],
	CONCAT([a].[StreetName], ' ', [a].[StreetNumber], ', ', [a].[City], ', ', [a].[PostCode], ', ', [co].[Name]) AS [Address]
FROM [Clients] AS [c]
INNER JOIN [Addresses] AS [a]
ON [c].[AddressId] = [a].[Id]
INNER JOIN [Countries] AS [co]
ON [a].[CountryId] = [co].[Id]
WHERE [c].[Id] NOT IN (
	SELECT [c].[Id]
	FROM [ProductsClients] AS [pc]
	INNER JOIN [Clients] AS [c]
	ON [pc].[ClientId] = [c].[Id]
)
ORDER BY [c].[Name] ASC