# .NET Core Authentication
This example utilizes the \[Authorize\] attribute to restrict access based on authentication for API and MVC views.

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order records
- Interact with Ordered Items, Products, Categories through API
- Confirm that Add, Delete, Update operations on Categories are blocked if not logged in
- Create an account on the sample app
- Use the network tab to monitor log in request (Look for Response Header: Set Cookie)
- Use developer tools to see local storage / cookies
- Observe new requests to the API after logging in
- Notice the request headers include the same cookie, and Add, Delete, Update work
- Try the same exercise with the MVC Category pages.

## Index of Examples
1. [Core Entity Framework](https://github.com/christinebittle/CoreEntityFramework)
2. [Core API](https://github.com/christinebittle/CoreAPI)
3. [Core Services](https://github.com/christinebittle/CoreServices)
4. [MVC & ViewModels](https://github.com/christinebittle/OnlineStore)
5. [Simple Authentication](https://github.com/christinebittle/OnlineStore/tree/Authentication1)
6. [Image/File Upload](https://github.com/christinebittle/OnlineStore/tree/product-image-upload)
7. [Role Based Authorization](https://github.com/christinebittle/OnlineStore/tree/Authentication2)
8. [Conditional Access](https://github.com/christinebittle/OnlineStore/tree/conditional-access)
9. [Conditional Rendering](https://github.com/christinebittle/OnlineStore/tree/conditional-rendering)
10. [Client Integration](https://github.com/christinebittle/OnlineStore/tree/client-integration)


## Test Your Understanding!
- Restrict the OrderItems API to use \[Authorize\] on Delete operatiohn
- Send a requst to delete with no authentication token, confirm the record is not deleted.
- Log in, copy the authentication cookie
- Introduce the authentication cookie into the request Header (see CategoryController.cs documentation)
- Confirm the request to add the order item is now allowed
- Restrict POST requests to /Add, /Delete, /Update/\{id\} order item in OrderItemPageController.cs with \[Authorize\]
- Use the network tab to confirm the behaviors:
- Confirm that the actions are blocked if unauthenticated (no cookie sent)
- Confirm the actions are allowed if authenticated (cookie sent)
- Restrict GET requests to /New, /DeleteConfirm/\{id\}, /Edit/\{id\}
- Use the network tab to confirm the restricted behaviors
