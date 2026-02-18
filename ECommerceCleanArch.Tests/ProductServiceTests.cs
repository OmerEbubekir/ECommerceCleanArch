using Application.DTOs;
using Application.Helpers; // Pagination burada
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications; // SpecParams burada
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ECommerceCleanArch.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepo = new Mock<IGenericRepository<Product>>();

            _mockUnitOfWork.Setup(u => u.Repository<Product>()).Returns(_mockProductRepo.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnPagination_WhenParamsAreValid()
        {
            // 1. ARRANGE
            // Kullanýcýdan gelen parametreler (Sayfa 1, Boyut 10)
            var specParams = new ProductSpecParams { PageIndex = 1, PageSize = 10 };

            var products = new List<Product>
            {
                new Product("Test Phone", "Desc", 100, 1),
                new Product("Test Laptop", "Desc", 200, 1)
            };

            var productDtos = new List<ProductDto>
            {
                new ProductDto { Name = "Test Phone" },
                new ProductDto { Name = "Test Laptop" }
            };

            // SENARYO 1: ListAllAsync artýk (skip, take, includes) alýyor.
            // Skip: 0, Take: 10 bekliyoruz.
            _mockProductRepo.Setup(repo => repo.ListAllAsync(0, 10, It.IsAny<Expression<Func<Product, object>>[]>()))
                            .ReturnsAsync(products);

            // SENARYO 2: Service içinde CountAsync çaðrýlýyor, onu da mocklamalýyýz!
            _mockProductRepo.Setup(repo => repo.CountAsync())
                            .ReturnsAsync(2); // Toplam 2 ürün var diyelim.

            _mockMapper.Setup(m => m.Map<IReadOnlyList<ProductDto>>(products))
                       .Returns(productDtos);

            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            // 2. ACT
            // Artýk parametre gönderiyoruz!
            var result = await productService.GetProductsAsync(specParams);

            // 3. ASSERT
            Assert.NotNull(result);
            Assert.Equal(1, result.PageIndex); // 1. sayfada mýyýz?
            Assert.Equal(2, result.Count);     // Toplam sayý doðru mu?
            Assert.Equal(2, result.Data.Count); // Data listesinin içi dolu mu?
            Assert.Equal("Test Phone", result.Data[0].Name);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCallRepositoryAdd_WhenDtoIsValid()
        {
            // Bu testte bir deðiþiklik yok, aynen kalabilir.
            var createDto = new CreateProductDto { Name = "New", Price = 100, CategoryId = 1 };
            var productEntity = new Product("New", "Desc", 100, 1);
            var resultDto = new ProductDto { Id = 1, Name = "New" };

            _mockMapper.Setup(m => m.Map<Product>(createDto)).Returns(productEntity);
            _mockMapper.Setup(m => m.Map<ProductDto>(productEntity)).Returns(resultDto);
            _mockProductRepo.Setup(r => r.AddAsync(productEntity)).ReturnsAsync(productEntity);
            _mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(1);

            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            var result = await productService.CreateProductAsync(createDto);

            _mockProductRepo.Verify(r => r.AddAsync(productEntity), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldSoftDelete_WhenProductExists()
        {
            // Bu testte de deðiþiklik yok çünkü GetByIdAsync imzasýný (bool parametreli) koruduk.
            int productId = 1;
            var existingProduct = new Product("Test", "Desc", 10, 1);
            existingProduct.ID = productId;

            _mockProductRepo.Setup(r => r.GetByIdAsync(productId, It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                            .ReturnsAsync(existingProduct);

            _mockProductRepo.Setup(r => r.DeleteAsync(existingProduct))
                            .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(1);

            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            var result = await productService.DeleteProductAsync(productId);

            Assert.True(result);
            _mockProductRepo.Verify(r => r.DeleteAsync(It.Is<Product>(p => p.ID == productId)), Times.Once);
        }
    }
}