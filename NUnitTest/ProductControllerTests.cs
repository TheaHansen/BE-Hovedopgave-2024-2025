using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_Hovedopgave_2024_2025.Model;
using BE_Hovedopgave_2024_2025.Controllers;
using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Profiles;
using BE_Hovedopgave_2024_2025.Services;

//Setup and teardown are made together
namespace NUnitTest
{
    [TestFixture]
    public class ProductControllerTests
    {
        private OdontologicDbContext _context;
        private ProductController _controller;
        private IProductService _productService;
        private ILabelService _labelService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            //Use inMemory database
            var options = new DbContextOptionsBuilder<OdontologicDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();

            });
            
            _context = new OdontologicDbContext(options);
            
            _mapper = mapperConfig.CreateMapper();

            _productService = new ProductService(_mapper, _context);
            
            _labelService = new LabelService(_context);
            
            _controller = new ProductController(_productService, _labelService); 
            
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
                ArticleNumber = "Article12",
                Title = "Sample Product",
                ShortDescription = "Sample description",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                InCarousel = false,
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
            Assert.That(result, Is.InstanceOf<ActionResult<ProductDTO>>());
            
            // Checks that the value inside the ActionResult is of type Product
            // result.Value is the actual data returned (Product)
            Assert.That(result.Value, Is.TypeOf<ProductDTO>());
            
            // Checks that the returned product is the same as the one added to the database
            Assert.That(product.Id, Is.EqualTo(result.Value.Id));
            Assert.That(product.Title, Is.EqualTo(result.Value.Title));
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
            Assert.That(result, Is.InstanceOf<ActionResult<ProductDTO>>());
            
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
                ArticleNumber = "Article12",
                Title = "Sample Product",
                ShortDescription = "Sample description",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                InCarousel = false,
                Price = 10,
                Labels = new List<Label> {label1},
                Stocks = []
            };
            
            var product2 = new Product
            {
                Id = 2,
                ArticleNumber = "Article12",
                Title = "Sample Product",
                ShortDescription = "Sample description",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                InCarousel = false,
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
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<ProductDTO>>>());

            var returnedProduct = result.Value.FirstOrDefault();
            Assert.That(returnedProduct, Is.Not.Null);
            Assert.That(returnedProduct.Id, Is.EqualTo(product1.Id));
            Assert.That(returnedProduct.Labels[0].Id, Is.EqualTo(product1.Labels[0].Id));
            Assert.That(returnedProduct.Labels[0].Name, Is.EqualTo(product1.Labels[0].Name));
            

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
                ArticleNumber = "Article12",
                Title = "Sample Product",
                ShortDescription = "Sample description",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                InCarousel = false,
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
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<ProductDTO>>>());

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
                ArticleNumber = "Article12",
                Title = "Sample Product",
                ShortDescription = "Sample description",
                Description = "Sample Product",
                ImageUrl = "www.Sample-Product.fr",
                InCarousel = false,
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
            
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<ProductDTO>>>());

            // Checks that the result.Result is an instance of NotFoundObjectResult
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            
            //notFoundResult.Value is the response body
            Assert.That(notFoundResult.Value, Is.EqualTo($"Label '{labelNotFound}' not found"));
            

        }
        
    }
}