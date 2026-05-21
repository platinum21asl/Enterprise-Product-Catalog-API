using FluentValidation;
using ProductCatalog.Application.Models.DTOs.Req;

namespace ProductCatalog.Application.Validators.Products
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequestDto>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MinimumLength(3).WithMessage("SKU must be at least 3 characters.")
                .MaximumLength(20).WithMessage("SKU must not exceed 20 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
        }
    }
}