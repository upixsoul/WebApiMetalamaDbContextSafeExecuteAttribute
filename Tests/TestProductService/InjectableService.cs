using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Core;
using WebApiMetalamaDbContextSafeExecuteAttribute;

namespace TestProductService
{
    public class InjectableService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public DbContext? DbContext { get; private set; }

        public InjectableService(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [DbContextSafeExecute<AppDbContext>]
        public async Task<bool> IsContextInjectedAsync()
        {
            await Task.Delay(10); // Simula trabajo
            return DbContext != null;
        }
    }
}
