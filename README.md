# .NET Core Conditional Access
This example includes a worker that periodically calls [openai.com](Open AI) API to generate product descriptions.

> [!WARNING]
> This example is only meant to demonstrate server to server connection. The AI Generated content created is entirely fictional. Instructor / Institution is not liable for any generated content from the example, it is for entertainment purposes only.

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Add Customer, Order records
- Interact with Ordered Items, Products, Categories through API
- Create an account on the sample app
- Add 'admin' Role in AspNetRoles
- Associate AspNetUser with admin AspNetRole in AspNetUserRoles
- Interact with products as an admin vs. non admin
- Add 5 or more product records with images
- If desired, get a developer key from Open AI (usage is not free)
- Running the project will periodically update products with AI descriptions
  



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
11. [Workers and Server to Server Communication](https://github.com/christinebittle/OnlineStore/tree/worker)

## Test Your Understanding!
- Create your own worker that independently logs a 'Hello' message every thirty seconds

