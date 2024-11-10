# .NET Core Conditional Access
This example continues with conditional access to Orders

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
- Create Orders and OrderItems associated with customers
- Access Orders through customer vs. admin accounts

## Index of Examples
1. [Core Entity Framework](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)
3. [Core Services](https://github.com/christinebittle/CoreServices)
4. [MVC & ViewModels](https://github.com/christinebittle/OnlineStore)
5. [Simple Authentication](https://github.com/christinebittle/OnlineStore/tree/Authentication1)
6. [Image/File Upload](https://github.com/christinebittle/OnlineStore/tree/product-image-upload)
7. [Role Based Authorization](https://github.com/christinebittle/OnlineStore/tree/Authentication2)
8. [Conditional Access](https://github.com/christinebittle/OnlineStore/tree/conditional-access)

## Test Your Understanding!
- Modify "Show Product" page so that only Admins can access the order items for a product

