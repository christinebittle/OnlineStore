﻿using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;

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
                ProductDto ProductDto = new ProductDto()
                {
                    ProductId = Product.ProductId,
                    ProductName = Product.ProductName,
                    ProductSKU = Product.ProductSKU,
                    ProductPrice = Product.ProductPrice,
                    HasProductPic = Product.HasPic
                };
                

                if (Product.HasPic)
                {
                    ProductDto.ProductImagePath = $"/images/products/{Product.ProductId}{Product.PicExtension}";
                }

                // create new instance of ProductDto, add to list
                ProductDtos.Add(ProductDto);
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
                ProductPrice = Product.ProductPrice,
                HasProductPic = Product.HasPic
            };
            if (Product.HasPic)
            {
                ProductDto.ProductImagePath = $"/images/products/{Product.ProductId}{Product.PicExtension}";
            }

            return ProductDto;

        }


        public async Task<ServiceResponse> UpdateProduct(ProductDto ProductDto)
        {
            ServiceResponse serviceResponse = new();

            Product? Product = await _context.Products.FindAsync(ProductDto.ProductId);

            if (Product==null)
            {
                serviceResponse.Messages.Add("Product could not be found");
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                return serviceResponse;
            }

            Product.ProductName = ProductDto.ProductName;
            Product.ProductPrice = ProductDto.ProductPrice;
            Product.ProductSKU = ProductDto.ProductSKU;

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

            if (ProductPic.Length > 0)
            {
               

                // remove old picture if exists
                if (Product.HasPic)
                {
                    string OldFileName = $"{Product.ProductId}{Product.PicExtension}";
                    string OldFilePath = Path.Combine("wwwroot/images/products/", OldFileName);
                    if (File.Exists(OldFilePath))
                    {
                        File.Delete(OldFilePath);
                    }
                   
                }


                // establish valid file types (can be changed to other file extensions if desired!)
                List<string> Extensions = new List<string>{ ".jpeg", ".jpg", ".png", ".gif" };
                string ProductPicExtension = Path.GetExtension(ProductPic.FileName).ToLowerInvariant();
                if (!Extensions.Contains(ProductPicExtension))
                {
                    response.Messages.Add($"{ProductPicExtension} is not a valid file extension");
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    return response;
                }

                string FileName = $"{id}{ProductPicExtension}";
                string FilePath = Path.Combine("wwwroot/images/products/", FileName);

                using (var targetStream = File.Create(FilePath))
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
                        // SQL Equivalent: Update Products set HasPic=True, PicExtension={ext} where ProductId={id}

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
