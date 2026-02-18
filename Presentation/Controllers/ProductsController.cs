using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    // [ApiController]: Bu attribute, validasyon hatalarını otomatik yakalar (400 Bad Request döner).
    // [Route]: URL yapısını belirler. [controller] yerine sınıfın adı (Products) gelir.
    // Örn: https://localhost:5001/api/products
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor Injection: Servisi içeri alıyoruz.
        // Asla "new ProductService()" demiyoruz!
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            // 200 OK döner ve veriyi JSON olarak basar.
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            // Eğer ürün yoksa NULL dönmek yerine 404 dönmek profesyonelliktir.
            if (product == null)
            {
                return NotFound($"Product with id {id} not found.");
            }

            return Ok(product);
        }
    }
}