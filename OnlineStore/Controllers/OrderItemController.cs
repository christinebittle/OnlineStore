using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreEntityFramework;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        // dependency injection of service interfaces
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }


        /// <summary>
        /// Returns a list of Ordered Items, each represented by an OrderItemDto with their associated Product, Order, and Customer
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderItemDto},{OrderItemDto},..]
        /// </returns>
        /// <example>
        /// GET: api/OrderItems/List -> [{OrderItemDto},{OrderItemDto},..]
        /// </example>
        [HttpGet(template: "List")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> ListOrderItems()
        {
            // returns a list of order item dtos
            IEnumerable<OrderItemDto> orderItemDtos = await _orderItemService.ListOrderItems();
            // return 200 OK with OrderItemDtos
            return Ok(orderItemDtos);
        }

        /// <summary>
        /// Returns a single Ordered Item specified by its {id}, represented by an Order Item Dto with its associated Product, Order, and Customer
        /// </summary>
        /// <param name="id">The ordered item id</param>
        /// <returns>
        /// 200 OK
        /// {OrderItemDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/OrderItems/Find/1 -> {OrderItemDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<OrderItemDto>> FindOrderItem(int id)
        {
            // include will join order(i)tem with 1 product, 1 order, 1 customer
            // first or default async will get the first order(i)tem matching the {id}
            var orderItem = await _orderItemService.FindOrderItem(id);

            // if the item could not be located, return 404 Not Found
            if (orderItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(orderItem);
            }
        }

        /// <summary>
        /// Updates an Ordered Item
        /// </summary>
        /// <param name="id">The ID of Order Item to update</param>
        /// <param name="orderItemDto">The required information to update the ordered item (OrderItemId, OrderItemUnitPrice,OrderItemQty,ProductId,OrderId)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateOrderItem(int id, OrderItemDto orderItemDto)
        {
            // {id} in URL must match OrderItemId in POST Body
            if (id != orderItemDto.OrderItemId)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _orderItemService.UpdateOrderItem(orderItemDto);

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
        /// Adds an Order Item
        /// </summary>
        /// <param name="orderItemDto">The required information to add the ordered item (OrderItemUnitPrice,OrderItemQty,ProductId,OrderId)</param>
        /// <example>
        /// POST api/OrderItem/Add
        /// </example>
        /// <returns>
        /// 201 Created
        /// Location: api/OrderItem/Find/{OrderItemId}
        /// {OrderItemDto}
        /// or
        /// 404 Not Found
        /// </returns>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<OrderItem>> AddOrderItem(OrderItemDto orderItemDto)
        {
            ServiceResponse response = await _orderItemService.AddOrderItem(orderItemDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/OrderItem/Find/{response.CreatedId}", orderItemDto);
        }

        /// <summary>
        /// Deletes the Ordered Item
        /// </summary>
        /// <param name="id">The id of the Order Item to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteOrderItem(int id)
        {
            ServiceResponse response = await _orderItemService.DeleteOrderItem(id);

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


        //ListOrderItemsForOrder
        [HttpGet(template: "ListForOrder/{id}")]
        public async Task<IActionResult> ListOrderItemsForOrder(int id)
        {
            // empty list of data transfer object OrderItemDto
            IEnumerable<OrderItemDto> orderItemDtos = await _orderItemService.ListOrderItemsForOrder(id);
            // return 200 OK with OrderItemDtos
            return Ok(orderItemDtos);
        }


        //ListOrderItemsForProduct
        [HttpGet(template: "ListForProduct/{id}")]
        public async Task<IActionResult> ListOrderItemsForProduct(int id)
        {
            // empty list of data transfer object OrderItemDto
            IEnumerable<OrderItemDto> orderItemDtos = await _orderItemService.ListOrderItemsForProduct(id);
            // return 200 OK with OrderItemDtos
            return Ok(orderItemDtos);
        }


    }
}
