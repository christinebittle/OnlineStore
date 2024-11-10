using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Services
{
    public class OrderItemService : IOrderItemService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // dependency injection of database context
        public OrderItemService(AppDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IEnumerable<OrderItemDto>> ListOrderItems()
        {
            // include will join the order(i)tem with 1 product, 1 order, 1 customer
            List<OrderItem> orderItems = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .ToListAsync();
            // empty list of data transfer object OrderItemDto
            List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
            // foreach Order Item record in database
            foreach (OrderItem orderItem in orderItems)
            {
                // create new instance of OrderItemDto, add to list
                orderItemDtos.Add(new OrderItemDto()
                {
                    OrderItemId = orderItem.OrderItemId,
                    OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                    OrderItemQty = orderItem.OrderItemQty,
                    OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                    ProductId = orderItem.ProductId,
                    ProductSKU = orderItem.Product.ProductSKU,
                    OrderId = orderItem.OrderId,
                    OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = orderItem.Order.Customer.UserName
                });
            }
            // return OrderItemDtos
            return orderItemDtos;

        }


        public async Task<OrderItemDto?> FindOrderItem(int id)
        {
            // include will join order(i)tem with 1 product, 1 order, 1 customer
            // first or default async will get the first order(i)tem matching the {id}
            var orderItem = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .FirstOrDefaultAsync(i => i.OrderItemId == id);

            // no order item found
            if (orderItem == null)
            {
                return null;
            }
            // create an instance of orderItemDto
            OrderItemDto orderItemDto = new OrderItemDto()
            {
                OrderItemId = orderItem.OrderItemId,
                OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                OrderItemQty = orderItem.OrderItemQty,
                OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                ProductId = orderItem.ProductId,
                ProductSKU = orderItem.Product.ProductSKU,
                OrderId = orderItem.OrderId,
                OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                CustomerName = orderItem.Order.Customer.UserName
            };
            return orderItemDto;

        }


        public async Task<ServiceResponse> UpdateOrderItem(OrderItemDto orderItemDto)
        {
            ServiceResponse serviceResponse = new();
            Product? product = await _context.Products.FindAsync(orderItemDto.ProductId);
            Order? order = await _context.Orders.FindAsync(orderItemDto.OrderId);
            // Posted data must link to valid entity
            if (product == null || order == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                //404 Not Found
                return serviceResponse;
            }

            // Create instance of OrderItem
            OrderItem orderItem = new OrderItem()
            {
                OrderItemId = Convert.ToInt32(orderItemDto.OrderItemId),
                OrderItemUnitPrice = orderItemDto.OrderItemUnitPrice,
                OrderItemQty = orderItemDto.OrderItemQty,
                ProductId = orderItemDto.ProductId,
                Product = product,
                Order = order,
                OrderId = orderItemDto.OrderId
            };
            // flags that the object has changed
            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Orderitems set ... where OrderItemId={id}
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


        public async Task<ServiceResponse> AddOrderItem(OrderItemDto orderItemDto)
        {
            ServiceResponse serviceResponse = new();
            Product? product = await _context.Products.FindAsync(orderItemDto.ProductId);
            Order? order = await _context.Orders.FindAsync(orderItemDto.OrderId);

            // Data must link to a valid entity
            if (product == null || order == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (product == null)
                {
                    serviceResponse.Messages.Add("Product was not found. ");
                }
                if (order == null)
                {
                    serviceResponse.Messages.Add("Order was not found.");
                }
                return serviceResponse;
            }

            OrderItem orderItem = new OrderItem()
            {
                OrderItemUnitPrice = orderItemDto.OrderItemUnitPrice,
                OrderItemQty = orderItemDto.OrderItemQty,
                ProductId = orderItemDto.ProductId,
                Product = product,
                Order = order,
                OrderId = orderItemDto.OrderId
            };
            // SQL Equivalent: Insert into orderitems (..) values (..)

            try
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Order Item.");
                serviceResponse.Messages.Add(ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = orderItem.OrderItemId;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteOrderItem(int id)
        {
            ServiceResponse response = new();
            // Order Item must exist in the first place
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Order Item cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting order item");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }

        public async Task<IEnumerable<OrderItemDto>> ListOrderItemsForOrder(int id)
        {
            IdentityUser? User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string customerId = User.Id;
            bool isUserAdmin = await _userManager.IsInRoleAsync(User, "admin");

            

            // WHERE orderid == id
            List<OrderItem> orderItems = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .Where(i => i.OrderId == id)
                .ToListAsync();

            // empty list of data transfer object OrderItemDto
            List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
            // foreach Order Item record in database
            foreach (OrderItem orderItem in orderItems)
            {

                // conditional access to order item - admin or customer who made the order
                if ((orderItem.Order.Customer.Id != customerId) && !isUserAdmin) continue;
                

                // create new instance of OrderItemDto, add to list
                orderItemDtos.Add(new OrderItemDto()
                {
                    OrderItemId = orderItem.OrderItemId,
                    OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                    OrderItemQty = orderItem.OrderItemQty,
                    OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                    ProductId = orderItem.ProductId,
                    ProductSKU = orderItem.Product.ProductSKU,
                    OrderId = orderItem.OrderId,
                    OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = orderItem.Order.Customer.UserName
                });
            }
            // return 200 OK with OrderItemDtos
            return orderItemDtos;

        }

        public async Task<IEnumerable<OrderItemDto>> ListOrderItemsForProduct(int id)
        {
            // WHERE productid == id
            List<OrderItem> orderItems = await _context.OrderItems
                .Include(i => i.Product)
                .Include(i => i.Order)
                .Include(i => i.Order.Customer)
                .Where(i => i.ProductId == id)
                .ToListAsync();

            // empty list of data transfer object OrderItemDto
            List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
            // foreach Order Item record in database
            foreach (OrderItem orderItem in orderItems)
            {
                // create new instance of OrderItemDto, add to list
                orderItemDtos.Add(new OrderItemDto()
                {
                    OrderItemId = orderItem.OrderItemId,
                    OrderItemUnitPrice = orderItem.OrderItemUnitPrice,
                    OrderItemQty = orderItem.OrderItemQty,
                    OrderItemSubtotal = orderItem.OrderItemQty * orderItem.OrderItemUnitPrice,
                    ProductId = orderItem.ProductId,
                    ProductSKU = orderItem.Product.ProductSKU,
                    OrderId = orderItem.OrderId,
                    OrderDate = orderItem.Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = orderItem.Order.Customer.UserName
                });
            }
            // return 200 OK with OrderItemDtos
            return orderItemDtos;

        }
    }
}
