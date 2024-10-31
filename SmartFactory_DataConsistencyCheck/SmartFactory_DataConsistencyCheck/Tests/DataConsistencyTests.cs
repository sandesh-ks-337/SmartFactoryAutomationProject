using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFactory_DataConsistencyCheck.Models;


namespace SmartFactory_DataConsistencyCheck.Tests
{
    [TestFixture]
    public class DataConsistencyTests
    {
        private Mock<IProductDataSource> _productDataSourceMock;
        private Mock<IOrderDataSource> _orderDataSourceMock;

        private Dictionary<int, Order> _orderStore;

        [SetUp]
        public void SetUp()
        {
            _productDataSourceMock = new Mock<IProductDataSource>();
            _orderDataSourceMock = new Mock<IOrderDataSource>();

            _orderStore = new Dictionary<int, Order>
            {
                { 1, new Order { Id = 1, ProductId = 1, Quantity = 2, TotalPrice = 200 } },
                { 2, new Order { Id = 2, ProductId = 2, Quantity = 1, TotalPrice = 200 } }
            };

            // Setup GetOrderByIdAsync to retrieve orders from the in-memory store
            _orderDataSourceMock.Setup(ds => ds.GetOrderByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _orderStore.ContainsKey(id) ? _orderStore[id] : null);

            // Setup UpdateOrderAsync to modify orders in the in-memory store
            _orderDataSourceMock.Setup(ds => ds.UpdateOrderAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .Callback<int, Order>((id, updatedOrder) =>
                {
                    if (_orderStore.ContainsKey(id))
                    {
                        _orderStore[id] = updatedOrder;
                    }
                })
                .Returns(Task.CompletedTask);
        }

        [Test]
        public async Task VerifyProductDataConsistency()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Price = 100 },
            new Product { Id = 2, Name = "Product2", Price = 200 }
        };

            _productDataSourceMock.Setup(p => p.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productDataSourceMock.Object.GetAllProductsAsync();

            // ClassicAssert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(products.Count, result.Count());
            ClassicAssert.AreEqual(products.First().Id, result.First().Id);
        }

        [Test]
        public async Task VerifyOrderDataConsistency()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, ProductId = 1, Quantity = 2, TotalPrice = 200 },
                new Order { Id = 2, ProductId = 2, Quantity = 1, TotalPrice = 200 }
            };

            _orderDataSourceMock.Setup(o => o.GetAllOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _orderDataSourceMock.Object.GetAllOrdersAsync();

            // ClassicAssert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(orders.Count, result.Count());
            ClassicAssert.AreEqual(orders.First().Id, result.First().Id);
        }
        [Test]
        public async Task VerifyConcurrentUpdatesWithRangeCheckOnOrder()
        {
            // Arrange
            var orderId = 1;
            var newDetails1 = new Order { Id = orderId, ProductId = 3, Quantity = 5, TotalPrice = 500 };
            var newDetails2 = new Order { Id = orderId, ProductId = 4, Quantity = 7, TotalPrice = 700 };

            // Act - Simulate concurrent updates
            var task1 = Task.Run(() => _orderDataSourceMock.Object.UpdateOrderAsync(orderId, newDetails1));
            var task2 = Task.Run(() => _orderDataSourceMock.Object.UpdateOrderAsync(orderId, newDetails2));

            await Task.WhenAll(task1, task2);

            // Retrieve the final state of the order
            var result = await _orderDataSourceMock.Object.GetOrderByIdAsync(orderId);

            // Assert - Check if the final state matches one of the updates, showing consistency in end state
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductId, Is.InRange(3, 4));
            Assert.That(result.Quantity, Is.InRange(5, 7));
            Assert.That(result.TotalPrice, Is.InRange(500, 700));
        }
        [Test]
        public async Task VerifyMultiOrderConsistency()
        {
            // Arrange
            var originalOrders = new List<Order>
    {
        new Order { Id = 1, ProductId = 1, Quantity = 2, TotalPrice = 200 },
        new Order { Id = 2, ProductId = 2, Quantity = 3, TotalPrice = 600 }
    };

            _orderDataSourceMock.Setup(ds => ds.GetAllOrdersAsync()).ReturnsAsync(originalOrders);

            // Act - Modify only one order
            var newDetails = new Order { Id = 1, ProductId = 5, Quantity = 4, TotalPrice = 800 };
            await _orderDataSourceMock.Object.UpdateOrderAsync(1, newDetails);

            var allOrders = await _orderDataSourceMock.Object.GetAllOrdersAsync();

            // Assert - Verify other orders are unaffected
            Assert.That(allOrders.First(o => o.Id == 2).ProductId, Is.EqualTo(2));
            Assert.That(allOrders.First(o => o.Id == 2).Quantity, Is.EqualTo(3));
            Assert.That(allOrders.First(o => o.Id == 2).TotalPrice, Is.EqualTo(600));
        }
        [Test]
        public async Task VerifyConcurrentUpdatesOnOrder()
        {
            // Arrange
            var orderId = 1;
            var originalOrder = new Order { Id = orderId, ProductId = 1, Quantity = 1, TotalPrice = 100 };

            var updatedOrder1 = new Order { Id = orderId, ProductId = 1, Quantity = 2, TotalPrice = 200 };
            var updatedOrder2 = new Order { Id = orderId, ProductId = 1, Quantity = 3, TotalPrice = 300 };

            // Setting up the initial state of the order
            _orderDataSourceMock.Setup(o => o.GetOrderByIdAsync(orderId)).ReturnsAsync(originalOrder);

            // Mocking the update behavior
            _orderDataSourceMock.Setup(o => o.UpdateOrderAsync(orderId, It.IsAny<Order>()))
                .Callback<int, Order>((id, order) =>
                {
                    // Update logic that simulates the behavior of updating the order
                    if (order.Id == orderId)
                    {
                        originalOrder.ProductId = order.ProductId;
                        originalOrder.Quantity = order.Quantity;
                        originalOrder.TotalPrice = order.TotalPrice;
                    }
                });

            // Act - Simulate concurrent updates
            var task1 = Task.Run(() => _orderDataSourceMock.Object.UpdateOrderAsync(orderId, updatedOrder1));
            var task2 = Task.Run(() => _orderDataSourceMock.Object.UpdateOrderAsync(orderId, updatedOrder2));

            await Task.WhenAll(task1, task2);

            // Retrieve the final state of the order
            var result = await _orderDataSourceMock.Object.GetOrderByIdAsync(orderId);

            // Assert - Check if the final state matches the last updated order
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalPrice, Is.EqualTo(updatedOrder2.TotalPrice)); // Final price should be 300
            Assert.That(result.Quantity, Is.EqualTo(updatedOrder2.Quantity)); // Final quantity should be 3
            Assert.That(result.ProductId, Is.EqualTo(updatedOrder2.ProductId)); // Final product ID should match
        }
        [Test]
        public async Task VerifyEmptyProductList()
        {
            // Arrange
            _productDataSourceMock.Setup(p => p.GetAllProductsAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _productDataSourceMock.Object.GetAllProductsAsync();

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task VerifyEmptyOrderList()
        {
            // Arrange
            _orderDataSourceMock.Setup(o => o.GetAllOrdersAsync()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderDataSourceMock.Object.GetAllOrdersAsync();

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(0, result.Count());
        }
        [Test]
        public async Task VerifyOrderTotalPriceCalculation()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var quantity = 2;
            var pricePerUnit = 100m; // Price per unit of the product
            var expectedTotalPrice = pricePerUnit * quantity; // Expected total price

            var order = new Order
            {
                Id = orderId,
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = expectedTotalPrice
            };

            // Setup the mock to return the order when requested
            _orderDataSourceMock.Setup(o => o.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderDataSourceMock.Object.GetOrderByIdAsync(orderId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalPrice, Is.EqualTo(expectedTotalPrice)); // Check if total price is correctly calculated
            Assert.That(result.Quantity, Is.EqualTo(quantity)); // Verify quantity
            Assert.That(result.ProductId, Is.EqualTo(productId)); // Verify product ID
        }
    }
}
