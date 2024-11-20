using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_Hovedopgave_2024_2025.Model;
using BE_Hovedopgave_2024_2025.Controllers;

//Setup and teardown are made together
namespace NUnitTest
{
    [TestFixture]
    public class ProductControllerTests
    {
        private OdontologicDbContext _context;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            //Use inMemory database
            var options = new DbContextOptionsBuilder<OdontologicDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            _context = new OdontologicDbContext(options);
            _controller = new ProductController(_context); 
        }
        
        //Cleans DB after each test
        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        //Made together
        [Test]
        public async Task GetProduct_ProductExists_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Title = "Sample Product",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                Price = 10,
                Labels = [],
                Stocks = []
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            //Checks that the result is an action result with a Product
            Assert.That(result, Is.InstanceOf<ActionResult<Product>>());
            
            // Checks that the value inside the ActionResult is of type Product
            // result.Value is the actual data returned (Product)
            Assert.That(result.Value, Is.TypeOf<Product>());
            
            // Checks that the returned product is the same as the one added to the database
            Assert.That(product, Is.EqualTo(result.Value));
        }
        
        //Made together
        [Test]
        public async Task GetProduct_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var productId = 1;

            // Act
            // A ActionResult with a NotFoundObject
            var result = await _controller.GetProduct(productId);

            // Assert
            //Checks that the result is an action result with a Product
            Assert.That(result, Is.InstanceOf<ActionResult<Product>>());
            
            // Checks that the result.Result is an instance of NotFoundObjectResult
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            
            //notFoundResult.Value is the response body
            Assert.That(notFoundResult.Value, Is.EqualTo("Product not found"));
        }
        
    }
}