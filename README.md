# .NET Core Client Integration
This example includes client-side integration with the [Lightbox2](https://lokeshdhakar.com/projects/lightbox2/) plugin and [tinymce](https://www.tiny.cloud/).

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
- Explore editing product descriptions with HTML and JS content
- Get an API key with TinyMCE
- Remove the sanitization in ProductService
- Remove reference to TinyMCE to edit text area directly
- Observe that product description is now vulnerable to XSS by injection JS into the input
  

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
- Translate another field into a rich text field, such as the category description
- Add your own client script integration into this example project!

