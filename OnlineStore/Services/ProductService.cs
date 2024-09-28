using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace CoreEntityFramework.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        // dependency injection of database context
        public ProductService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ProductDto>> ListProducts()
        {
            // all Products
            List<Product> Products = await _context.Products
                .ToListAsync();
            // empty list of data transfer object ProductDto
            List<ProductDto> ProductDtos = new List<ProductDto>();
            // foreach Order Item record in database
            foreach (Product Product in Products)
            {
                // create new instance of ProductDto, add to list
                ProductDtos.Add(new ProductDto()
                {
                    ProductId = Product.ProductId,
                    ProductName = Product.ProductName,
                    ProductSKU = Product.ProductSKU,
                    ProductPrice = Product.ProductPrice
                });
            }
            // return ProductDtos
            return ProductDtos;

        }


        public async Task<ProductDto?> FindProduct(int id)
        {
            // include will join order(i)tem with 1 product, 1 order, 1 customer
            // first or default async will get the first order(i)tem matching the {id}
            var Product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            // no order item found
            if (Product == null)
            {
                return null;
            }
            // create an instance of ProductDto
            ProductDto ProductDto = new ProductDto()
            {
                ProductId = Product.ProductId,
                ProductName = Product.ProductName,
                ProductSKU = Product.ProductSKU,
                ProductPrice = Product.ProductPrice
            };
            return ProductDto;

        }


        public async Task<ServiceResponse> UpdateProduct(ProductDto ProductDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Product
            Product Product = new Product()
            {
                ProductId = ProductDto.ProductId,
                ProductName = ProductDto.ProductName,
                ProductSKU = ProductDto.ProductSKU,
                ProductPrice = ProductDto.ProductPrice
            };
            // flags that the object has changed
            _context.Entry(Product).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Products set ... where ProductId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");
                
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }


        public async Task<ServiceResponse> AddProduct(ProductDto ProductDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Product
            Product Product = new Product()
            {
                ProductName = ProductDto.ProductName,
                ProductSKU = ProductDto.ProductSKU,
                ProductPrice = ProductDto.ProductPrice
            };
            // SQL Equivalent: Insert into Products (..) values (..)

            try
            {
                _context.Products.Add(Product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Product.");
                serviceResponse.Messages.Add(ex.Message);
                
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Product.ProductId;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteProduct(int id)
        {
            ServiceResponse response = new();
            // Order Item must exist in the first place
            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Product cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Product");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }

        public async Task<IEnumerable<ProductDto>> ListProductsForCategory(int id)
        {
            // join CategoryProduct on Products.Productid = CategoryProduct.Productid WHERE CategoryProduct.categoryid = {id}
            List<Product> Products = await _context.Products
                .Where(p => p.Categories.Any(c => c.CategoryId == id))
                .ToListAsync();

            // empty list of data transfer object ProductDto
            List<ProductDto> ProductDtos = new List<ProductDto>();
            // foreach Order Item record in database
            foreach (Product Product in Products)
            {
                // create new instance of ProductDto, add to list
                ProductDtos.Add(new ProductDto()
                {
                    ProductId = Product.ProductId,
                    ProductName = Product.ProductName,
                    ProductSKU = Product.ProductSKU,
                    ProductPrice = Product.ProductPrice
                });
            }
            // return ProductDtos
            return ProductDtos;

        }


    }
}
