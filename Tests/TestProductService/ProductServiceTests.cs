using Microsoft.EntityFrameworkCore;
using ProductService.Core;
using ProductService.Core.Models;

namespace TestProductService
{
    [TestFixture]
    public class ProductServiceTests
    {
        private AppDbContext _context = null!;
        private ProductService.Core.ProductService _productService = null!;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            var factory = new TestDbContextFactory(options);
            _productService = new ProductService.Core.ProductService(factory);

            _context = new AppDbContext(options);

            await _context.Products.AddRangeAsync(new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1200 },
                new Product { Id = 2, Name = "Mouse", Price = 25 },
                new Product { Id = 3, Name = "", Price = 10 }
            });

            await _context.SaveChangesAsync();

            _productService = new ProductService.Core.ProductService(factory);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [Test]
        public void GetAvailableProductsAsync_ShouldReturnFilteredProducts()
        {
            var result = _productService.GetAvailableProducts();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            //Assert.IsTrue(result.All(p => !string.IsNullOrEmpty(p.Name)));
            Assert.That(result.All(p => !string.IsNullOrEmpty(p.Name)));
        }
        
        [Test]
        public async Task AddProductAsync_ShouldPersistProduct()
        {
            var newProduct = new Product { Id = 4, Name = "Keyboard", Price = 45 };

            _productService.AddProduct(newProduct);

            var products = await _context.Products.ToListAsync();
            Assert.That(products.Count, Is.EqualTo(4));
            Assert.That(products.Any(p => p.Name == "Keyboard"));
        }

        [Test]
        public async Task Aspect_ShouldExecuteDecoratedMethod()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AspectTestDb_" + Guid.NewGuid())
                .Options;

            var factory = new TestDbContextFactory(options);
            var service = new ProductService.Core.ProductService(factory);

            service.AddProduct(new Product { Id = 1, Name = "Tablet", Price = 300 });

            var context = factory.CreateDbContext();
            var products = await context.Products.ToListAsync();

            Assert.That(products.Count, Is.EqualTo(1));
            Assert.That(products[0].Name, Is.EqualTo("Tablet"));
        }

        [Test]
        public void Aspect_ShouldInjectDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InjectTestDb_" + Guid.NewGuid())
                .Options;

            var factory = new TestDbContextFactory(options);
            var service = new InjectableService(factory);

            var result = service.IsContextInjected();

            Assert.That(result, Is.True);
        }

        [Test]
        public void Aspect_ShouldReturnDefaultOnException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AspectErrorTestDb_" + Guid.NewGuid())
                .Options;

            var factory = new TestDbContextFactory(options);
            var service = new FaultyProductService(factory);

            var result = service.GetAvailableProducts();

            Assert.That(result, Is.Null);// El aspecto debería devolver default
        }

    }
}
