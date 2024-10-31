using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using SmartFactory_ITOT_Integration.Controllers;
using SmartFactory_ITOT_Integration.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework.Legacy;

namespace SmartFactory_API_Tests.Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new OrderController();
        }

        [Test]
        public void GetAllOrders_ShouldReturnListOfOrders()
        {
            // Arrange
            var order1 = new Order { Id = 1, ProductId = 1, Quantity = 2 };
            var order2 = new Order { Id = 2, ProductId = 2, Quantity = 3 };

            // Create the expected list of orders
            var expectedOrders = new List<Order> { order1, order2 };

            // Add the expected orders to the controller
            _controller.CreateOrder(order1);
            _controller.CreateOrder(order2);

            // Act
            var result = _controller.GetAllOrders();

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsInstanceOf<ActionResult<IEnumerable<Order>>>(result);
            var okResult = result.Result as OkObjectResult;
            var orders = okResult?.Value as IEnumerable<Order>;

            ClassicAssert.IsNotNull(orders);
            ClassicAssert.AreEqual(expectedOrders.Count, orders.Count());
        }

        [Test]
        public void GetOrderById_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, ProductId = 1, Quantity = 2 };
            _controller.CreateOrder(order);

            // Act
            var result = _controller.GetOrderById(1);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsInstanceOf<ActionResult<Order>>(result);
            var okResult = result.Result as OkObjectResult;
            var retrievedOrder = okResult?.Value as Order;

            ClassicAssert.IsNotNull(retrievedOrder);
            ClassicAssert.AreEqual(order.Id, retrievedOrder?.Id);
        }

        [Test]
        public void GetOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Act
            var result = _controller.GetOrderById(99); // Assuming this ID does not exist

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsInstanceOf<ActionResult<Order>>(result);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }


        [Test]
        public void CreateOrder_ShouldReturnCreatedOrder()
        {
            // Arrange
            var order = new Order { ProductId = 1, Quantity = 2 };

            // Act
            var result = _controller.CreateOrder(order);

            // ClassicAssert
            ClassicAssert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            ClassicAssert.IsInstanceOf<Order>(createdResult.Value);
            var createdOrder = (Order)createdResult.Value;
            ClassicAssert.AreEqual(order.ProductId, createdOrder.ProductId);
        }

        [Test]
        public void UpdateOrder_ShouldReturnNoContent_WhenOrderExists()
        {
            // Arrange
            var order = new Order { ProductId = 1, Quantity = 2 };
            var createdResult = _controller.CreateOrder(order);
            var createdOrder = createdResult.Result as CreatedAtActionResult;
            var orderId = ((Order)createdOrder.Value).Id;

            var updatedOrder = new Order { ProductId = 2, Quantity = 5 };

            // Act
            var result = _controller.UpdateOrder(orderId, updatedOrder);

            // ClassicAssert
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void UpdateOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var updatedOrder = new Order { ProductId = 2, Quantity = 5 };

            // Act
            var result = _controller.UpdateOrder(999, updatedOrder); // Assume this ID does not exist

            // ClassicAssert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void DeleteOrder_ShouldReturnNoContent_WhenOrderExists()
        {
            // Arrange
            var order = new Order { ProductId = 1, Quantity = 2 };
            var createdResult = _controller.CreateOrder(order);
            var createdOrder = createdResult.Result as CreatedAtActionResult;
            var orderId = ((Order)createdOrder.Value).Id;

            // Act
            var result = _controller.DeleteOrder(orderId);

            // ClassicAssert
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void DeleteOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Act
            var result = _controller.DeleteOrder(999); // Assume this ID does not exist

            // ClassicAssert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}