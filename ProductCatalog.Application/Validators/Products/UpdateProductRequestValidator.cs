using FluentValidation;
using ProductCatalog.Application.Models.DTOs.Req;

namespace ProductCatalog.Application.Validators.Products
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequestDto>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product ID is required for update.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
        }
    }
}