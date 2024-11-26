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
            
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        
        //Cleans context after each test
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
            // An ActionResult with a NotFoundObject
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
        
        //Made together
        [Test]
        public async Task GetProduct_ByLabel_ReturnsProducts()
        {
            var label1 = new Label { Name = "tilbud" };
            var label2 = new Label { Name = "tøj" };
            
            var product1 = new Product
            {
                Id = 1,
                Title = "Sample Product",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                Price = 10,
                Labels = new List<Label> {label1},
                Stocks = []
            };
            
            var product2 = new Product
            {
                Id = 2,
                Title = "Sample Product",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                Price = 10,
                Labels = new List<Label> {label2},
                Stocks = []
            };
            
            _context.Labels.Add(label1);
            _context.Labels.Add(label2);
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _controller.GetProductsByLabel("tilbud");
            
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Count, Is.EqualTo(1));
            Console.WriteLine(result.Value);
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Product>>>());

            var returnedProduct = result.Value.FirstOrDefault();
            Assert.That(returnedProduct, Is.Not.Null);
            Assert.That(returnedProduct.Id, Is.EqualTo(product1.Id));
            Assert.That(returnedProduct.Labels, Is.EquivalentTo(product1.Labels));
            

        }
        
        //Made together
        [Test]
        public async Task GetProduct_ByLabel_ReturnsProductNotFound()
        {
            var label1 = new Label { Name = "tilbud" };
            var label2 = new Label { Name = "tøj" };
            
            var product1 = new Product
            {
                Id = 1,
                Title = "Sample Product",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                Price = 10,
                Labels = new List<Label> {label1},
                Stocks = []
            };
            
            
            _context.Labels.Add(label1);
            _context.Labels.Add(label2);
            _context.Products.Add(product1);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _controller.GetProductsByLabel("tøj");
            
            Assert.That(result.Value, Is.Null);
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Product>>>());

            // Checks that the result.Result is an instance of NotFoundObjectResult
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            
            //notFoundResult.Value is the response body
            Assert.That(notFoundResult.Value, Is.EqualTo($"Product not found with '{label2.Name}'"));
            

        }
        
        //Made together
        [Test]
        public async Task GetProduct_ByLabel_ReturnsLabelNotFound()
        {
            var label1 = new Label { Name = "tilbud" };
            string labelNotFound = "tøj";
            
            var product1 = new Product
            {
                Id = 1,
                Title = "Sample Product",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                Price = 10,
                Labels = new List<Label> {label1},
                Stocks = []
            };
            
            _context.Labels.Add(label1);
            _context.Products.Add(product1);
            await _context.SaveChangesAsync();
            
            // Act
            var result = await _controller.GetProductsByLabel(labelNotFound);
            
            Assert.That(result.Value, Is.Null);
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Product>>>());

            // Checks that the result.Result is an instance of NotFoundObjectResult
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            
            //notFoundResult.Value is the response body
            Assert.That(notFoundResult.Value, Is.EqualTo($"Label '{labelNotFound}' not found"));
            

        }
        
    }
}