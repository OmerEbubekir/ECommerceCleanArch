using Application.DTOs;
using Application.Interfaces;
using Core.Specifications;
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
        // [FromQuery]: Verileri URL'den (?pageIndex=1...) oku demektir.
        public async Task<IActionResult> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var result = await _productService.GetProductsAsync(productParams);
            return Ok(result);
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            // Validasyon burada otomatik çalışır.
            // Eğer kurala uymazsa, Controller metoduna girmeden 400 Bad Request döner.

            var newProduct = await _productService.CreateProductAsync(createProductDto);

            // 201 Created: "Başarıyla oluşturuldu" standardıdır.
            // Response Header'da yeni ürünün URL'ini de döneriz (nameof(GetProductById)).
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut] // Güncelleme için PUT kullanılır.
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                

                await _productService.UpdateProductAsync(updateProductDto);
                return NoContent(); // 204 No Content: "İşlem başarılı ama sana dönecek verim yok."
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Şimdilik basit hata dönüşü.
            }   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound($"Product with id {id} not found.");
            }

            // 204 No Content: Başarıyla sildim (gizledim), dönecek veri yok.
            return NoContent();
        }
    }
}