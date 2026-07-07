USE [Accounting]

GO

SELECT 
	[c].[Id],
	[c].[Name] AS [Client],
	CONCAT([a].[StreetName], ' ', [a].[StreetNumber], ', ', [a].[City], ', ', [a].[PostCode], ', ', [co].[Name]) AS [Address]
FROM [Clients] AS [c]
FULL JOIN [ProductsClients] AS [pc]
ON [pc].[ClientId] = [c].[Id]
INNER JOIN [Addresses] AS [a]
ON [c].[AddressId] = [a].[Id]
INNER JOIN [Countries] AS [co]
ON [a].[CountryId] = [co].[Id]
WHERE [pc].[ClientId] IS NULL
ORDER BY [c].[Name] ASC