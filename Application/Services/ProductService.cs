using AutoMapper; 
using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
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
            var products = await _unitOfWork.Repository<Product>().ListAllAsync(false,p => p.Category);
            // 2. Entity -> DTO Dönüşümü (AutoMapper ile)
            // Okunuşu: "products listesini al, IReadOnlyList<ProductDto> tipine çevir."
            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id,false, p => p.Category);

            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // 1. DTO -> Entity Dönüşümü
            
            var productEntity = _mapper.Map<Product>(createProductDto);

            // 2. Veritabanına Ekle (Hafızada)
            await _unitOfWork.Repository<Product>().AddAsync(productEntity);

            // 3. Kaydet (SQL'e Git)
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null; // Kayıt başarısızsa

            // 4. Oluşan Entity'yi -> DTO'ya çevirip geri dön (Client ID'yi görsün diye)
            return _mapper.Map<ProductDto>(productEntity);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            

            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateProductDto.Id);

            if (product == null)
            {
                throw new Exception("Ürün bulunamadı"); 
            }

            // 2. DTO'daki verilerle Entity'i güncelle (AutoMapper ile)
            _mapper.Map(updateProductDto, product);

            // 3. Update metodunu çağır (State'i Modified yapar)
            await _unitOfWork.Repository<Product>().UpdateAsync(product);

            // 4. Kaydet
            await _unitOfWork.Complete();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // 1. Ürünü Bul
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            if (product == null) return false; // Veya exception fırlatılabilir.

            // 2. Soft Delete uygula
            await _unitOfWork.Repository<Product>().DeleteAsync(product);

            // 3. Kaydet
            var result = await _unitOfWork.Complete();

            return result > 0;
        }
    }
}