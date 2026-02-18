using Application.DTOs;
using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ECommerceCleanArch.Tests
{
    public class ProductServiceTests
    {
        // Mock nesneleri: Gerçek servislerin yerine geçecek taklitler.
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGenericRepository<Product>> _mockProductRepo;

        public ProductServiceTests()
        {
            // Her testten önce bu nesneler sýfýrdan oluþturulur.
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepo = new Mock<IGenericRepository<Product>>();

            // UnitOfWork'e "Biri senden Repository<Product> isterse, benim sahte repomu ver" diyoruz.
            _mockUnitOfWork.Setup(u => u.Repository<Product>()).Returns(_mockProductRepo.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnProductList_WhenProductsExist()
        {
            // 1. ARRANGE (Hazýrlýk)
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

            // Senaryo: Repository'nin ListAllAsync metodu çaðrýldýðýnda, yukarýdaki sahte listeyi dön.
            // It.IsAny<Expression...>() kýsmý: "Include parametresi ne gelirse gelsin fark etmez" demek.
            _mockProductRepo.Setup(repo => repo.ListAllAsync(false,It.IsAny<Expression<Func<Product, object>>[]>()))
                            .ReturnsAsync(products);

            // Senaryo: Mapper çaðrýldýðýnda sahte DTO listesini dön.
            _mockMapper.Setup(m => m.Map<IReadOnlyList<ProductDto>>(products))
                       .Returns(productDtos);

            // Servisi oluþtur (Sahte nesnelerle)
            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            // 2. ACT (Eylem)
            var result = await productService.GetProductsAsync();

            // 3. ASSERT (Doðrulama)
            Assert.NotNull(result); // Sonuç boþ gelmemeli.
            Assert.Equal(2, result.Count); // 2 adet ürün gelmeli.
            Assert.Equal("Test Phone", result[0].Name); // Ýlk ürünün adý doðru mu?
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCallRepositoryAdd_WhenDtoIsValid()
        {
            // 1. ARRANGE (Hazýrlýk)
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 100,
                CategoryId = 1
            };

            var productEntity = new Product("New Product", "Desc", 100, 1);
            var resultDto = new ProductDto { Id = 1, Name = "New Product" };

            // Mapper Setup: CreateDto gelince Product Entity dön
            _mockMapper.Setup(m => m.Map<Product>(createDto)).Returns(productEntity);

            // Mapper Setup: Product Entity gelince ProductDto dön (Return deðeri için)
            _mockMapper.Setup(m => m.Map<ProductDto>(productEntity)).Returns(resultDto);

            // Repository Setup: AddAsync çaðrýldýðýnda baþarýlý say.
            _mockProductRepo.Setup(r => r.AddAsync(productEntity)).ReturnsAsync(productEntity);

            // UnitOfWork Setup: Complete çaðrýldýðýnda 1 (baþarýlý) dön.
            _mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(1);

            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            // 2. ACT (Eylem)
            var result = await productService.CreateProductAsync(createDto);

            // 3. ASSERT (Doðrulama)
            // Repository'nin AddAsync metodu KESÝN olarak 1 kere çaðrýldý mý?
            _mockProductRepo.Verify(r => r.AddAsync(productEntity), Times.Once);

            // UnitOfWork'ün Complete metodu KESÝN olarak 1 kere çaðrýldý mý?
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(resultDto.Id, result.Id);
        }


        [Fact]
        public async Task DeleteProductAsync_ShouldSoftDelete_WhenProductExists()
        {
            // 1. ARRANGE
            int productId = 1;
            var existingProduct = new Product("Test", "Desc", 10, 1);
            existingProduct.ID = productId; // ID'yi eþle

            // HATALI OLAN KISIM BURASIYDI:
            // _mockProductRepo.Setup(r => r.GetByIdAsync(productId, false, null)).ReturnsAsync(existingProduct);

            // DÜZELTÝLMÝÞ HALÝ:
            // It.IsAny<bool>() -> True da gelse false da gelse kabul et.
            // It.IsAny<Expression...>() -> Include gelse de gelmese de kabul et.
            _mockProductRepo.Setup(r => r.GetByIdAsync(productId, It.IsAny<bool>(), It.IsAny<Expression<Func<Product, object>>[]>()))
                            .ReturnsAsync(existingProduct);

            // Delete (Update) metodu çaðrýldýðýnda hiçbir þey yapma (Task tamamla)
            _mockProductRepo.Setup(r => r.DeleteAsync(existingProduct))
                            .Returns(Task.CompletedTask);

            // UnitOfWork Complete çaðrýlýnca 1 dön (Baþarýlý)
            _mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(1);

            var productService = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);

            // 2. ACT
            var result = await productService.DeleteProductAsync(productId);

            // 3. ASSERT
            Assert.True(result); // Þimdi True dönmeli.

            // Verify ederken de esnek davranýyoruz
            _mockProductRepo.Verify(r => r.DeleteAsync(It.Is<Product>(p => p.ID == productId)), Times.Once);
        }

    }
}