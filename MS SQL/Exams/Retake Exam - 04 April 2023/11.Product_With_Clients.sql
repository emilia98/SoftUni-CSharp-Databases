USE [Accounting]

GO

CREATE FUNCTION [udf_ProductWithClients](@name NVARCHAR(35))
	RETURNS INT
			 AS
		  BEGIN
		 RETURN (
				SELECT COUNT([pc].[ProductId])
				FROM [Products] AS p
				INNER JOIN [ProductsClients] AS [pc]
				ON [p].[Id] = [pc].[ProductId]
				WHERE [p].[Name] = @name
				GROUP BY [pc].[ProductId]
		 )
			END

GO

SELECT dbo.udf_ProductWithClients('DAF FILTER HU12103X')