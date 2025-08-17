using Microsoft.EntityFrameworkCore;
using ProductService.Core;
using ProductService.Core.Models;

namespace TestProductService
{
    public partial class FaultyProductService : IProductService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public FaultyProductService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [DbContextSafeExecute<AppDbContext>]
        public virtual async Task<List<Product>> GetAvailableProductsAsync()
        {
            await Task.Delay(10); // Simula trabajo
            throw new InvalidOperationException("Simulated failure");
        }

        [DbContextSafeExecute<AppDbContext>]
        public async Task AddProductAsync(Product product)
        {
            await Task.Delay(10); // Simula trabajo
            throw new InvalidOperationException("Simulated failure");
        }
    }
}
