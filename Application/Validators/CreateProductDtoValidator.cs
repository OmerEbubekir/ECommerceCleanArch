using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(100).WithMessage("Ürün adı 100 karakteri geçemez.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok negatif olamaz."); // 0 olabilir

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçmelisiniz.");
        }
    }
}