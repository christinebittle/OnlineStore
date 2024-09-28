using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreEntityFramework.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        // dependency injection of database context
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CategoryDto>> ListCategories()
        {
            // all categories
            List<Category> Categories = await _context.Categories
                .ToListAsync();
            // empty list of data transfer object CategoryDto
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();
            // foreach Order Item record in database
            foreach (Category Category in Categories)
            {
                // create new instance of CategoryDto, add to list
                CategoryDtos.Add(new CategoryDto()
                {
                    CategoryId = Category.CategoryId,
                    CategoryName = Category.CategoryName,
                    CategoryDescription = Category.CategoryDescription,
                    CategoryColor = Category.CategoryColor
                });
            }
            // return CategoryDtos
            return CategoryDtos;

        }


        public async Task<CategoryDto?> FindCategory(int id)
        {
            // include will join order(i)tem with 1 product, 1 order, 1 customer
            // first or default async will get the first order(i)tem matching the {id}
            var Category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            // no order item found
            if (Category == null)
            {
                return null;
            }
            // create an instance of CategoryDto
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName,
                CategoryDescription = Category.CategoryDescription,
                CategoryColor = Category.CategoryColor
            };
            return CategoryDto;

        }


        public async Task<ServiceResponse> UpdateCategory(CategoryDto CategoryDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Category
            Category Category = new Category()
            {
                CategoryId = CategoryDto.CategoryId,
                CategoryName = CategoryDto.CategoryName,
                CategoryDescription = CategoryDto.CategoryDescription,
                CategoryColor = CategoryDto.CategoryColor
            };
            // flags that the object has changed
            _context.Entry(Category).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Categories set ... where CategoryId={id}
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


        public async Task<ServiceResponse> AddCategory(CategoryDto CategoryDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Category
            Category Category = new Category()
            {
                CategoryName = CategoryDto.CategoryName,
                CategoryDescription = CategoryDto.CategoryDescription,
                CategoryColor = CategoryDto.CategoryColor
            };
            // SQL Equivalent: Insert into Categories (..) values (..)

            try
            {
                _context.Categories.Add(Category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Category.");
                serviceResponse.Messages.Add(ex.Message);
                
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Category.CategoryId;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteCategory(int id)
        {
            ServiceResponse response = new();
            // Order Item must exist in the first place
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Category cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the category");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }

        public async Task<IEnumerable<CategoryDto>> ListCategoriesForProduct(int id)
        {
            // join CategoryProduct on categories.categoryid = CategoryProduct.categoryid WHERE CategoryProduct.productid = {id}
            List<Category> Categories = await _context.Categories
                .Where(c => c.Products.Any(p => p.ProductId == id))
                .ToListAsync();

            // empty list of data transfer object CategoryDto
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();
            // foreach Order Item record in database
            foreach (Category Category in Categories)
            {
                // create new instance of CategoryDto, add to list
                CategoryDtos.Add(new CategoryDto()
                {
                    CategoryId = Category.CategoryId,
                    CategoryName = Category.CategoryName,
                    CategoryDescription = Category.CategoryDescription,
                    CategoryColor = Category.CategoryColor
                });
            }
            // return CategoryDtos
            return CategoryDtos;

        }

        public async Task<ServiceResponse> LinkCategoryToProduct(int categoryId, int productId)
        {
            ServiceResponse serviceResponse = new();

            Category? category = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.CategoryId == categoryId)
                .FirstOrDefaultAsync();
            Product? product = await _context.Products.FindAsync(productId);

            // Data must link to a valid entity
            if (product == null || category == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (product == null)
                {
                    serviceResponse.Messages.Add("Product was not found. ");
                }
                if (category == null)
                {
                    serviceResponse.Messages.Add("Category was not found.");
                }
                return serviceResponse;
            }
            try
            {
                category.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue linking the product to the category");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }

        public async Task<ServiceResponse> UnlinkCategoryFromProduct(int categoryId, int productId)
        {
            ServiceResponse serviceResponse = new();

            Category? category = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.CategoryId == categoryId)
                .FirstOrDefaultAsync();
            Product? product = await _context.Products.FindAsync(productId);

            // Data must link to a valid entity
            if (product == null || category == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (product == null)
                {
                    serviceResponse.Messages.Add("Product was not found. ");
                }
                if (category == null)
                {
                    serviceResponse.Messages.Add("Category was not found.");
                }
                return serviceResponse;
            }
            try
            {
                category.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue unlinking the product to the category");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
