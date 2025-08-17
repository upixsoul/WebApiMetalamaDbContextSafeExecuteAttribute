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
    public List<Product> GetAvailableProducts()
    {
        var result = new List<Product>();
        using (var context = _contextFactory.CreateDbContext())
        {
            Console.WriteLine("context assigned in GetAvailableProductsAsync");
            result = context.Products
                .Where(p => !string.IsNullOrEmpty(p.Name))
                .ToListAsync().Result;
            Console.WriteLine("result assigned in GetAvailableProductsAsync");
        }
        return result;
    }

    [DbContextSafeExecute<AppDbContext>]
    public virtual void AddProduct(Product product)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            Console.WriteLine("context assigned in AddProductAsync");
            context.Products.Add(product);
            context.SaveChanges();
        }
    }
}

public interface IProductService
{
    public List<Product> GetAvailableProducts();
    public void AddProduct(Product product);
}