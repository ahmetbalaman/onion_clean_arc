using FluentValidation;

namespace OnionArcAndAll.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithName("Başlık")
                .NotNull()
                .MaximumLength(50);

            RuleFor(p => p.Description)
                .Length(10, 500);

            RuleFor(p => p.Price)
                .GreaterThan(0);

            RuleFor(p => p.Discount).
                GreaterThanOrEqualTo(0).
                LessThanOrEqualTo(100);

            RuleFor(p => p.BrandId)
                .NotEmpty()
                .NotNull();

            RuleFor(x=> x.CategoryIds).NotEmpty()
                .Must(categories => categories.Any())
                .WithName("Kategoriler");
        }
    }
}
