using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommandRequest>
    {
        public UpdateProductCommandValidator()
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

            RuleFor(x => x.CategoryIds).NotEmpty()
                .Must(categories => categories.Any())
                .WithName("Kategoriler");
        }
    }
}
