# .NET Core Authentication 2
This example introduces roles in ASP.NET Individual User Accounts, and how to manage access by a User's Role (admin, customer)

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order records
- Interact with Ordered Items, Products, Categories through API
- Create an account on the sample app
- Add 'customer' Role in AspNetRoles
- Add 'admin' Role in AspNetRoles
- Associate AspNetUser with admin AspNetRole in AspNetUserRoles
- Repeat steps, creating a non-admin user with a 'customer' role

## Index of Examples
1. [Core Entity Framework](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)
3. [Core Services](https://github.com/christinebittle/CoreServices)
4. [MVC & ViewModels](https://github.com/christinebittle/OnlineStore)
5. [Simple Authentication](https://github.com/christinebittle/OnlineStore/tree/Authentication1)
6. [Image/File Upload](https://github.com/christinebittle/OnlineStore/tree/product-image-upload)
7. [Role Based Authorization](https://github.com/christinebittle/OnlineStore/tree/Authentication2)

## Test Your Understanding!
- Only users with the 'administrator' role should access ListOrderItems
- Implement ListMyOrders in OrderService, using httpContextAccessor to get the current logged in user
- Implement API GET: Orders/ListMyOrders which is accessible for Customers
- Build a web page that allows users to see a list of their own Orders by calling the ListMyOrders method in the OrderService
- Consider what it would mean to manipulate OrderItems within the context of a Customer vs. an Admin, and the associated Authorization needed!
