# .NET Core Image Upload Feature
This example has an image uploading feature for Products

## To run this project
- Tools > NuGet Package Manager > Package Manager Console
- update-database
- Tools > SQL Server Object Explorer > Database
- Product Records
- **Create folder /images/products in wwwroot**
- Upload an image for the Product record through api/Product/UploadProductPic/{id}
- Confirm the existence of the image in the project through project files
- Upload new image for same product
- Look for database columns for product with picture, without picture

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
- Modify ProductDto on ListProducts, ShowProduct
- Use Views to show product information on /ProductPage/List, /ProductPage/Show/{id}
- Modify Product/Edit to have a form with enctype="multi-part/form-data", receive IFormFile and forward to UploadProductPicService
