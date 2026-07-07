USE [Accounting]

GO

CREATE PROCEDURE usp_SearchByCountry(@countryName NVARCHAR(10))
AS
BEGIN
	SELECT 
		[v].[Name] AS [Vendor],
		[v].[NumberVAT] AS [VAT],
		CONCAT([a].[StreetName], ' ', [a].[StreetNumber]) AS [Street Info],
		CONCAT([a].[City], ' ', [a].[PostCode]) AS [City Info]
	FROM [Vendors] AS [v]
	INNER JOIN [Addresses] AS [a]
	ON [v].[AddressId] = [a].[Id]
	INNER JOIN [Countries] AS [c]
	ON [a].[CountryId] = [c].[Id]
	WHERE [c].[Name] = @countryName
	ORDER BY [v].[Name] ASC, [a].[City] ASC
END

GO

EXEC usp_SearchByCountry 'France'