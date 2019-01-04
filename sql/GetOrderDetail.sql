
SELECT CustomerName,CustomerAddress,CustomerMobile,TotalAmount,TotalDiscount,Orders.CreatedDate,Orders.Description,Products.ID, Products.Name,OrderDetails.Quantity,Prices.SalePrice,Prices.PromotionPrice FROM dbo.Orders JOIN dbo.OrderDetails ON OrderDetails.OrderID = Orders.ID
JOIN dbo.Prices ON Prices.ID = OrderDetails.ProductID
JOIN dbo.Products ON Products.ID = Prices.ProductID


