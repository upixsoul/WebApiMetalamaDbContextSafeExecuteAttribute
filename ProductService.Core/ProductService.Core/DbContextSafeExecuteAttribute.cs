using Metalama.Framework.Aspects;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Models;

namespace ProductService.Core
{
    [CompileTime]
    public class DbContextSafeExecuteAttribute<TContext> : OverrideMethodAspect
    #pragma warning disable LAMA0236
        where TContext : DbContext
    #pragma warning restore LAMA0236
    {
        public override async Task<dynamic?> OverrideMethod()
        {
            Console.WriteLine("Metalama: Aspect triggered");

            var factory = meta.This.GetProperty<IDbContextFactory<TContext>>("_contextFactory");
            await using var context = await factory.CreateDbContextAsync();

            if (meta.This.Type.TryGetProperty("DbContext", out dynamic dbContextProp) &&
                dbContextProp.Type.IsAssignableTo(typeof(DbContext)))
            {
                dbContextProp.WithValue(context);
            }

            try
            {
                return await meta.ProceedAsync(); // Aquí ocurre la excepción
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Metalama ERROR] {ex.Message}");
                return (List<Product>?)default;
            }
        }
    }
}
