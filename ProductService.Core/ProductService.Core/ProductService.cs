using Microsoft.EntityFrameworkCore;
using ProductService.Core.Models;

namespace ProductService.Core;

public class ProductService : IProductService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ProductService(IDbContextFactory<AppDbContext> contextFactory)
    {
        Console.WriteLine("ProductService ctor");
        _contextFactory = contextFactory;
        Console.WriteLine("contextFactory assigned");
    }

    [DbContextSafeExecute<AppDbContext>]
    public async Task<List<Product>> GetAvailableProductsAsync()
    {
        var result = new List<Product>();
        await using (var context = await _contextFactory.CreateDbContextAsync())
        {
            Console.WriteLine("context assigned in GetAvailableProductsAsync");
            result = await context.Products
                .Where(p => !string.IsNullOrEmpty(p.Name))
                .ToListAsync();
            Console.WriteLine("result assigned in GetAvailableProductsAsync");
        }
        return result;
    }

    [DbContextSafeExecute<AppDbContext>]
    public virtual async Task AddProductAsync(Product product)
    {
        await using (var context = await _contextFactory.CreateDbContextAsync())
        {
            Console.WriteLine("context assigned in AddProductAsync");
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
    }
}

public interface IProductService
{
    public Task<List<Product>> GetAvailableProductsAsync();
    public Task AddProductAsync(Product product);
}