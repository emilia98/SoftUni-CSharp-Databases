USE [TechStore]

GO

-- Select statement to get all the affordable products
SELECT *
	FROM [Products]
	WHERE [Price] < 500

UPDATE [Products]
	SET [Price] = [Price] * 1.15
	FROM [Products]
	WHERE [Price] < 500