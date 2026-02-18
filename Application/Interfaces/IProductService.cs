using Application.DTOs;
using Application.Helpers; // Pagination sınıfı burada
using Core.Specifications; // ProductSpecParams burada
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        // ESKİSİNİ SİLDİK. Sadece bu kaldı:
        Task<Pagination<ProductDto>> GetProductsAsync(ProductSpecParams productParams);

        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
    }
}