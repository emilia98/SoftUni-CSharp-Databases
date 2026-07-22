USE [TechStore]

GO

SELECT 
	COUNT([m].[Id]) AS [ProductsCount]
	FROM [Manufacturers] AS [m]
	INNER JOIN [Products] AS [p]
	ON [m].[Id] = [p].[ManufacturerId]
	WHERE [m].[Name] = 'Apples'
	GROUP BY [m].[Id]

CREATE FUNCTION [udf_GetProductCountByManufacturer](@manufacturerName NVARCHAR(50))
	RETURNS INT
			 AS
		  BEGIN
		  DECLARE @manufacturerProducts INT
			SELECT 
				@manufacturerProducts = COUNT([m].[Id])
				FROM [Manufacturers] AS [m]
				INNER JOIN [Products] AS [p]
				ON [m].[Id] = [p].[ManufacturerId]
				WHERE [m].[Name] = @manufacturerName
				GROUP BY [m].[Id]
			
			IF(@manufacturerProducts IS NULL)
			BEGIN
				SET @manufacturerProducts = 0
			END

			RETURN @manufacturerProducts
		  END

GO
 
SELECT dbo.udf_GetProductCountByManufacturer ('Apple')