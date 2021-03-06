USE [Northwind]
GO
/****** Object:  StoredProcedure [dbo].[GetProducts]    Script Date: 18.12.2020 0:18:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetProducts] AS
BEGIN
SELECT
		Products.ProductID,
		Products.ProductName,
		Products.QuantityPerUnit,
		Products.UnitPrice,
		Products.UnitsOnOrder,
		Categories.CategoryName
FROM Orders 
INNER JOIN [Order Details] ON Orders.OrderID=[Order Details].OrderID  
INNER JOIN Products ON [Order Details].ProductID=Products.ProductID
INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID
INNER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID
ORDER BY Products.ProductID
END;