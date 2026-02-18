using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        
        Task<IReadOnlyList<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);

        Task UpdateProductAsync(UpdateProductDto updateProductDto);
    }
}