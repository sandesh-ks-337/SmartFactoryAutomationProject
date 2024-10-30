using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFactory_ITOT_Integration.Controllers;
using SmartFactory_ITOT_Integration.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFactory_API_Tests.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new ProductController();
            var productsField = typeof(ProductController).GetField("Products", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (productsField != null)
            {
                var products = (List<Product>)productsField.GetValue(null);
                products.Clear(); // Clear the list before each test
            }
        }

        [Test]
        public void GetAllProducts_ShouldReturnListOfProducts()
        {
            // Arrange
            var product1 = new Product { Name = "Product 1", Description = "Description 1", Price = 10.0m };
            var product2 = new Product { Name = "Product 2", Description = "Description 2", Price = 20.0m };
            _controller.CreateProduct(product1);
            _controller.CreateProduct(product2);

            // Act
            var result = _controller.GetAllProducts();

            // ClassicAssert
            ClassicAssert.IsInstanceOf<ActionResult<IEnumerable<Product>>>(result);
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult, "Expected OkObjectResult but received null.");

            var products = okResult.Value as IEnumerable<Product>;
            ClassicAssert.IsNotNull(products, "Expected a list of Product objects but received null.");
            ClassicAssert.AreEqual(2, products.Count());
        }

        [Test]
        public void GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Description = "Test Description", Price = 10.0m };
            _controller.CreateProduct(product);

            // Act
            var result = _controller.GetProductById(1); // Assuming ID 1 for the created product

            // ClassicAssert
            ClassicAssert.IsInstanceOf<ActionResult<Product>>(result);
            var okResult = result.Result as OkObjectResult; // Cast to OkObjectResult
            ClassicAssert.IsNotNull(okResult, "Expected OkObjectResult but received null.");

            var returnedProduct = okResult.Value as Product; // Access the Value property
            ClassicAssert.IsNotNull(returnedProduct, "Expected a Product object but received null.");
            ClassicAssert.AreEqual("Test Product", returnedProduct.Name);
            ClassicAssert.AreEqual("Test Description", returnedProduct.Description);
            ClassicAssert.AreEqual(10.0m, returnedProduct.Price);
        }

        [Test]
        public void GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = _controller.GetProductById(999); // Assuming ID 999 does not exist

            // ClassicAssert
            ClassicAssert.IsInstanceOf<ActionResult<Product>>(result);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void CreateProduct_ShouldReturnCreatedProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product", Description = "New Description", Price = 15.0m };

            // Act
            var result = _controller.CreateProduct(product);

            // ClassicAssert
            ClassicAssert.IsInstanceOf<ActionResult<Product>>(result);
            var createdResult = result.Result as CreatedAtActionResult; // Cast to CreatedAtActionResult
            ClassicAssert.IsNotNull(createdResult, "Expected CreatedAtActionResult but received null.");

            var returnedProduct = createdResult.Value as Product; // Access the Value property
            ClassicAssert.IsNotNull(returnedProduct, "Expected a Product object but received null.");
            ClassicAssert.AreEqual("New Product", returnedProduct.Name);
            ClassicAssert.AreEqual("New Description", returnedProduct.Description);
            ClassicAssert.AreEqual(15.0m, returnedProduct.Price);
        }

        [Test]
        public void UpdateProduct_ShouldReturnNoContent_WhenProductExists()
        {
            // Arrange
            var product = new Product { Name = "Old Product", Description = "Old Description", Price = 5.0m };
            _controller.CreateProduct(product);

            var updatedProduct = new Product { Name = "Updated Product", Description = "Updated Description", Price = 12.0m };

            // Act
            var result = _controller.UpdateProduct(1, updatedProduct); // Update product with ID 1

            // ClassicAssert
            ClassicAssert.IsInstanceOf<IActionResult>(result);
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var updatedProduct = new Product { Name = "Updated Product", Description = "Updated Description", Price = 12.0m };

            // Act
            var result = _controller.UpdateProduct(999, updatedProduct); // Update product with ID 999

            // ClassicAssert
            ClassicAssert.IsInstanceOf<IActionResult>(result);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void DeleteProduct_ShouldReturnNoContent_WhenProductExists()
        {
            // Arrange
            var product = new Product { Name = "Delete Product", Description = "Delete Description", Price = 8.0m };
            _controller.CreateProduct(product);

            // Act
            var result = _controller.DeleteProduct(1); // Delete product with ID 1

            // ClassicAssert
            ClassicAssert.IsInstanceOf<IActionResult>(result);
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = _controller.DeleteProduct(999); // Delete product with ID 999

            // ClassicAssert
            ClassicAssert.IsInstanceOf<IActionResult>(result);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}