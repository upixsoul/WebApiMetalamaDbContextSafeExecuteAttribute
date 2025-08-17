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
