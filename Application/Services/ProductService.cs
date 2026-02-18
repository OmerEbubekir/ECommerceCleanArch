using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
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

        // 1. GET ALL (PAGINATION)
        public async Task<Pagination<ProductDto>> GetProductsAsync(ProductSpecParams productParams)
        {
            var skip = (productParams.PageIndex - 1) * productParams.PageSize;
            var take = productParams.PageSize;

            // HATA BURADAYDI: Eskiden burada 'false' (bool) vardı. Onu sildik.
            // Artık sıra şöyle: Skip(int), Take(int), Includes(params)
            var products = await _unitOfWork.Repository<Product>()
                                            .ListAllAsync(skip, take, p => p.Category);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync();
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data);
        }

        // 2. GET BY ID
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            // Buradaki 'false' parametresi duruyor çünkü GetByIdAsync imzasında (int, bool, params) var.
            var product = await _unitOfWork.Repository<Product>()
                                           .GetByIdAsync(id, false, p => p.Category);

            if (product == null) return null;
            return _mapper.Map<ProductDto>(product);
        }

        // 3. CREATE
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var productEntity = _mapper.Map<Product>(createProductDto);
            await _unitOfWork.Repository<Product>().AddAsync(productEntity);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;
            return _mapper.Map<ProductDto>(productEntity);
        }

        // 4. UPDATE
        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(updateProductDto.Id);
            if (product == null) throw new Exception("Ürün bulunamadı");

            _mapper.Map(updateProductDto, product);
            await _unitOfWork.Repository<Product>().UpdateAsync(product);
            await _unitOfWork.Complete();
        }

        // 5. DELETE
        public async Task<bool> DeleteProductAsync(int id)
        {
            // Delete işleminde de 'ignoreQueryFilters: false' (varsayılan)
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null) return false;

            await _unitOfWork.Repository<Product>().DeleteAsync(product);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
    }
}