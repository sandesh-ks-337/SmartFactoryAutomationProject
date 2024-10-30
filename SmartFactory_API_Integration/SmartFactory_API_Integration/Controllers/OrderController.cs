using Microsoft.AspNetCore.Mvc;
using SmartFactory_ITOT_Integration.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFactory_ITOT_Integration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly List<Order> Orders = new List<Order>();

        // GET: /order
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            return Ok(Orders);
        }

        // GET: /order/{id}
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: /order
        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            if (order.Id == 0)
            {
                order.Id = Orders.Count > 0 ? Orders.Max(o => o.Id) + 1 : 1;
            }
            else if (Orders.Any(o => o.Id == order.Id))
            {
                return Conflict(new { message = $"Order with ID {order.Id} already exists." });
            }

            order.OrderDate = DateTime.UtcNow; // Set the order date
            Orders.Add(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // PUT: /order/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, Order updatedOrder)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.ProductId = updatedOrder.ProductId;
            order.Quantity = updatedOrder.Quantity;
            return NoContent(); // 204 No Content
        }

        // DELETE: /order/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Orders.Remove(order);
            return NoContent(); // 204 No Content
        }
    }
}