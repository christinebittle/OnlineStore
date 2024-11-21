using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore;
using OnlineStore.Models;
using OnlineStore.Interfaces;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CoreEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        // dependency injection of service interfaces
        public ProductController(IProductService ProductService)
        {
            _productService = ProductService;
        }


        /// <summary>
        /// Returns a list of Products. Pageable with optional parameters skip and page
        /// </summary>
        /// <param name="skip">The number of records to skip, ordered by ID ascending</param>
        /// <param name="page">The number of records to get</param>
        /// <returns>
        /// 200 OK
        /// [{ProductDto},{ProductDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Product/List -> [{ProductDto},{ProductDto},..]
        /// 
        /// GET: api/Product/List?start=0&perpage=100 -> [{ProductDto},{ProductDto},..+98]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> ListProducts(int? skip, int? page)
        {
            if (skip == null) skip = 0;
            if (page == null) page = await _productService.CountProducts();

            // empty list of data transfer object ProductDto
            IEnumerable <ProductDto> ProductDtos = await _productService.ListProducts((int)skip, (int)page);
            // return 200 OK with ProductDtos
            return Ok(ProductDtos);
        }

        /// <summary>
        /// Returns a single Product specified by its {id}
        /// </summary>
        /// <param name="id">The Product id</param>
        /// <returns>
        /// 200 OK
        /// {ProductDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Product/Find/1 -> {ProductDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<ProductDto>> FindProduct(int id)
        {

            var Product = await _productService.FindProduct(id);

            // if the Product could not be located, return 404 Not Found
            if (Product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Product);
            }
        }

        /// <summary>
        /// Updates a Product
        /// </summary>
        /// <param name="id">The ID of the Product to update</param>
        /// <param name="ProductDto">The required information to update the Product (ProductName, ProductColor)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Product/Update/5
        /// Request Headers: Content-Type: application/json
        /// Request Body: {ProductDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDto ProductDto)
        {
            // {id} in URL must match ProductId in POST Body
            if (id != ProductDto.ProductId)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _productService.UpdateProduct(ProductDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return NoContent();

        }

        /// <summary>
        /// Adds a Product
        /// </summary>
        /// <param name="ProductDto">The required information to add the Product (ProductName, ProductColor)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Product/Find/{ProductId}
        /// {ProductDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Product/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {ProductDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Product/Find/{ProductId}
        /// </example>
        [HttpPost(template: "Add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Product>> AddProduct(ProductDto ProductDto)
        {
            ServiceResponse response = await _productService.AddProduct(ProductDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Product/FindProduct/{response.CreatedId}", ProductDto);
        }

        /// <summary>
        /// Deletes the Product
        /// </summary>
        /// <param name="id">The id of the Product to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Product/Delete/7
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            ServiceResponse response = await _productService.DeleteProduct(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }



        /// <summary>
        /// Returns a list of products for a specific category by its {id}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{ProductDto},{ProductDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Product/ListForCategory/3 -> [{ProductDto},{ProductDto},..]
        /// </example>
        [HttpGet(template: "ListForCategory/{id}")]
        public async Task<ActionResult> ListProductsForCategory(int id)
        {
            // empty list of data transfer object ProductDto
            IEnumerable<ProductDto> ProductDtos = await _productService.ListProductsForCategory(id);
            // return 200 OK with CateDtos
            return Ok(ProductDtos);
        }


        /// <summary>
        /// Receives a product picture and saves it to /wwwroot/images/products/{id}{extension}
        /// </summary>
        /// <param name="id">The product to update an image for</param>
        /// <param name="ProductPic">The picture to change to</param>
        /// <returns>
        /// 200 OK
        /// or
        /// 404 NOT FOUND
        /// or 
        /// 500 BAD REQUEST
        /// </returns>
        /// <example>
        /// PUT : api/Product/UploadProductPic/2
        /// HEADERS: Content-Type: Multi-part/form-data, Cookie: .AspNetCore.Identity.Application={token}
        /// FORM DATA:
        /// ------boundary
        /// Content-Disposition: form-data; name="ProductPic"; filename="myproductpic.jpg"
        /// Content-Type: image/jpeg
        /// </example>
        /// <example>
        /// curl "https://localhost:xx/api/Product/UploadProductPic/1" -H "Cookie: .AspNetCore.Identity.Application={token}" -X "PUT" -F ProductPic=@myproductpic.jpg
        /// </example>
        [HttpPut(template: "UploadProductPic/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadProductPic(int id, IFormFile ProductPic)
        {

            ServiceResponse response = await _productService.UpdateProductImage(id, ProductPic);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Ok();

        }
    }
}
