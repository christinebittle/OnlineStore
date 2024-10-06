# .NET Core MVC & ViewModels
This example utilizes our services and dtos in the previous examples to organize and build webpages that facilitate content management. The Category Views are build using scaffolded code, where the Product Views are custom.

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order records
- Interact with Ordered Items, Products, Categories through webpages.

## Index of Examples
1. [Core Entity Framework](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)
3. [Core Services](https://github.com/christinebittle/CoreServices)
4. [MVC & ViewModels](https://github.com/christinebittle/OnlineStore)
5. [Simple Authentication](https://github.com/christinebittle/OnlineStore/tree/Authentication1)

## Test Your Understanding!
- Complete the CRUD for order items by building the Views which correspond to the OrderItemPageController methods (Find, New, Add, DeleteConfirm, Delete)
- Define an OrderItemNew.cs ViewModel which allows users to select which Order and which Product the Order Item refers to as a list.
- Modify the ProductDetails.cs ViewModel to include a list of ordered items for that product
- Modify the ProductPageController.cs Details method, using OrderItemService to receive a list of ordered items for a product (ListOrderItemsForProduct)
- Use the example as a scaffold to build an OrderPageController, which uses an OrderService to execute CRUD
- The OrderPageController Details method can use a ViewModel and the OrderItemService to ListOrderedItemsForOrder
- The OrderPageController Edit and New methods can use a ViewModel and the CustomerService, so the user can choose which customer the new / updated order belongs
