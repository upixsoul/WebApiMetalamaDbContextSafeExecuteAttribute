using Metalama.Framework.Aspects;
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

        [CatchError]
        public virtual List<Product> GetAvailableProducts()
        {
            throw new InvalidOperationException("Simulated failure");
        }

        [CatchError]
        public void AddProduct(Product product)
        {
            throw new InvalidOperationException("Simulated failure");
        }
    }

    [CompileTime]
    public class CatchErrorAttribute : OverrideMethodAspect
    {
        public override dynamic? OverrideMethod()
        {
            Console.WriteLine("Metalama: Aspect triggered");
            try
            {
                var result = meta.Proceed();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
