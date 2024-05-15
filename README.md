OfficeStore78
Приложение-магазин для продажи канцелярских товаров, которое будет реализовано с использованием C# совместно с MySQL. Для работы этого приложения потребуются следующие таблицы:

Products Table: This table will store information about all the products available in the store. The columns can include:
+ ProductID (Primary Key, Auto Increment)
+ ProductName
+ ProductDescription
+ ProductImage
+ ProductPrice
+ ProductCategoryID (Foreign Key)
  
Categories Table: This table will store information about the different categories of products available in the store. The columns can include:
+ CategoryID (Primary Key, Auto Increment)
+ CategoryName
  
 Orders Table: This table will store information about all the orders placed by customers. The columns can include:
+ OrderID (Primary Key, Auto Increment)
+ CustomerID (Foreign Key)
+ OrderDate
+ OrderStatus
  
OrderDetails Table: This table will store information about the products included in each order. The columns can include:
+ OrderDetailID (Primary Key, Auto Increment)
+ OrderID (Foreign Key)
+ ProductID (Foreign Key)
+ Quantity
+ Price
  
Customers Table: This table will store information about all the customers who have registered on the store. The columns can include:
+ CustomerID (Primary Key, Auto Increment)
+ FirstName
+ LastName
+ Email
+ Password
+ Address
+ PhoneNumber

Reviews Table: This table will store information about the reviews left by customers for the products. The columns can include:
+ ReviewID (Primary Key, Auto Increment)
+ ProductID (Foreign Key)
+ CustomerID (Foreign Key)
+ Rating
