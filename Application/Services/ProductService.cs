using AutoMapper; // Bu namespace gerekli
using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 
        
        
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductDto>> GetProductsAsync()
        {
            // 1. Veriyi Repository'den çek (Veritabanından Entity gelir)
            var products = await _unitOfWork.Repository<Product>().ListAllAsync(p => p.Category);
            // 2. Entity -> DTO Dönüşümü (AutoMapper ile)
            // Okunuşu: "products listesini al, IReadOnlyList<ProductDto> tipine çevir."
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id, p => p.Category);

            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }
    }
}