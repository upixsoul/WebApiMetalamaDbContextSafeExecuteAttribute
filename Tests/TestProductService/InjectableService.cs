using Microsoft.EntityFrameworkCore;
using ProductService.Core;

namespace TestProductService
{
    public class InjectableService
    {
        private readonly IDbContextFactory<AppDbContext>? _contextFactory = null;

        public DbContext? DbContext { get; private set; }

        public InjectableService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [DbContextSafeExecute<AppDbContext>]
        public bool IsContextInjected()
        {
            return _contextFactory != null;
        }
    }
}
