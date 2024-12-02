using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Drawing;
using System.Text.Encodings.Web;
using Ganss.Xss;


namespace CoreEntityFramework.Services
{
    public class ProductService : IProductService
    {

        // dependency injection database context 
        private readonly AppDbContext _context;
        // sanitize content to reduce XSS vulnerability
        private IHtmlSanitizer _htmlSanitizer;

        public ProductService(AppDbContext context, IHtmlSanitizer htmlSanitizer)
        {
            _context = context;
            _htmlSanitizer = htmlSanitizer;
        }


        public async Task<IEnumerable<ProductDto>> ListProducts(int skip, int page)
        {
            // MySQL equivalent:
            // select * from products order by productid asc limit {skip}, {page}
            // MSSQL equivalent:
            // select * from products order by productid asc offset {skip} rows fetch first {take} rows only;
            List<Product> Products = await _context.Products
                .OrderBy(p=>p.ProductId)
                .Skip(skip)
                .Take(page)
                .ToListAsync();
            // empty list of data transfer object ProductDto
            List<ProductDto> ProductDtos = new List<ProductDto>();
            // foreach Order Item record in database
            foreach (Product Product in Products)
            {
                string Image = "";
                if (Product.HasPic)
                {
                    Image = $"/images/products/{Product.ProductId}{Product.PicExtension}";
                }
                else
                {
                    Image = $"/images/products/default.jpg";
                }
                // create new instance of ProductDto, add to list
                ProductDtos.Add(new ProductDto()
                {
                    ProductId = Product.ProductId,
                    ProductName = Product.ProductName,
                    ProductSKU = Product.ProductSKU,
                    ProductPrice = Product.ProductPrice,
                    HasProductPic = Product.HasPic,
                    ProductImagePath = Image
                });

                
            }
            // return ProductDtos
            return ProductDtos;

        }

        public async Task<int> CountProducts()
        {
            return await _context.Products.CountAsync();
        }


        public async Task<ProductDto?> FindProduct(int id)
        {
            // One product matching the product id
            var Product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            // no order item found
            if (Product == null)
            {
                return null;
            }

            string Image = "";
            if (Product.HasPic)
            {
                Image = $"/images/products/{Product.ProductId}{Product.PicExtension}";
            }

            // create an instance of ProductDto
            ProductDto ProductDto = new ProductDto()
            {
                ProductId = Product.ProductId,
                ProductName = Product.ProductName,
                ProductSKU = Product.ProductSKU,
                ProductPrice = Product.ProductPrice,
                HasProductPic = Product.HasPic,
                ProductDescription = Product.ProductDescription,
                ai_generated=Product.ai_generated,
                ProductImagePath = Image
            };
            return ProductDto;

        }


        public async Task<ServiceResponse> UpdateProduct(ProductDto ProductDto)
        {
            ServiceResponse serviceResponse = new();

            Product? Product = await _context.Products.FindAsync(ProductDto.ProductId);

            if (Product == null)
            {
                serviceResponse.Messages.Add("Product could not be found");
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                return serviceResponse;
            }

            Product.ProductName = ProductDto.ProductName;
            Product.ProductPrice = ProductDto.ProductPrice;
            Product.ProductSKU = ProductDto.ProductSKU;
            Product.ProductDescription = _htmlSanitizer.Sanitize(ProductDto.ProductDescription);

            // Create instance of Product

            // flags that the object has changed
            _context.Entry(Product).State = EntityState.Modified;
            // handled by another method
            _context.Entry(Product).Property(p => p.HasPic).IsModified = false;
            _context.Entry(Product).Property(p => p.PicExtension).IsModified = false;

            try
            {
                // SQL Equivalent: Update Products set ... where ProductId={id}
                //_context.Products.Update(Product);
                //_context.SaveChanges();
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
                ProductPrice = ProductDto.ProductPrice,
                ProductDescription = _htmlSanitizer.Sanitize(ProductDto.ProductDescription)
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
            // Product must exist in the first place
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



        public async Task<ServiceResponse> UpdateProductImage(int id, IFormFile ProductPic)
        {
            ServiceResponse response = new();

            Product? Product = await _context.Products.FindAsync(id);
            if (Product==null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add($"Product {id} not found");
                return response;
            }

            if (ProductPic == null)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("No picture included");
                return response;
            };

            if (ProductPic.Length > 0)
            {
               

                // remove old picture if exists
                if (Product.HasPic)
                {
                    string OldFileName = $"{Product.ProductId}{Product.PicExtension}";
                    string OldFilePath = Path.Combine("wwwroot/images/products/", OldFileName);
                    if (File.Exists(OldFilePath))
                    {
                        System.IO.File.Delete(OldFilePath);
                    }
                   
                }


                //establish valid file types (can be changed to other file extensions if desired!)
                List<string> Extensions = new List<String>{ ".jpeg", ".jpg", ".png", ".gif" };
                string ProductPicExtension = Path.GetExtension(ProductPic.FileName).ToLowerInvariant();
                if (!Extensions.Contains(ProductPicExtension))
                {
                    response.Messages.Add($"{ProductPicExtension} is not a valid file extension");
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    return response;
                }

                string FileName = $"{id}{ProductPicExtension}";
                string FilePath = Path.Combine("wwwroot/images/products/", FileName);

                using (var targetStream = System.IO.File.Create(FilePath))
                {
                    ProductPic.CopyTo(targetStream);
                }

                // check if file was uploaded
                if (File.Exists(FilePath)) { 
                    Product.PicExtension = ProductPicExtension;
                    Product.HasPic = true;

                    _context.Entry(Product).State = EntityState.Modified;

                    try
                    {
                        // SQL Equivalent: Update Products set ... where ProductId={id}
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        response.Status = ServiceResponse.ServiceStatus.Error;
                        response.Messages.Add("An error occurred updating the record");

                        return response;
                    }
                }

            }
            else
            {
                response.Messages.Add("No File Content");
                response.Status = ServiceResponse.ServiceStatus.Error;
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Updated;
           


            return response;
        }


    }
}
