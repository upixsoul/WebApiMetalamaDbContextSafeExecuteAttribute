using Microsoft.AspNetCore.Mvc;
using ProductService.Core;
using ProductService.Core.Models;

namespace WebApiMetalamaDbContextSafeExecuteAttribute
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IProductService _productService;
        public ValuesController(IProductService productService)
        {
            _productService = productService;
            Console.WriteLine($"productService loaded");
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var values = new List<string>();
            var result = _productService.GetAvailableProducts();
            foreach (var product in result)
            {
                values.Add($"Id:{product.Id}, Name:{product.Name}, Price:{product.Price}");
            }
            return values;
        }


        // POST api/<ValuesController>
        [HttpPost("{name}")]
        public async Task PostAsync(string name)
        {
            _productService.AddProduct(new Product()
            {
                Name = name,
                Price = 123
            });
        }

    }
}