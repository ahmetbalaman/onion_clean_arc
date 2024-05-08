using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Features.Products.Rules;
using OnionArcAndAll.Application.Interfaces.AutoMapper;
using OnionArcAndAll.Application.Interfaces.UnitOfWorks;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : BaseHandler, IRequestHandler<CreateProductCommandRequest,Unit>
    {
        private readonly ProductRules productRules;

        public CreateProductCommandHandler(ProductRules productRules,IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.productRules = productRules;
        }

        public async Task<Unit> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            IList<Product> products = await unitOfWork.GetReadRepository<Product>().GetAllAsync();

            
            await productRules.ValidateProductTitle(products,request.Title);

            Product product = new(
                request.Title,request.Description,request.BrandId,request.Price,request.Discount

                );

            await unitOfWork.GetWriteRepository<Product>().AddAsync(product);
            if(await unitOfWork.SaveAsync() > 1)
            {
                foreach (var categoryId in request.CategoryIds)
                    await unitOfWork.GetWriteRepository<ProductCategory>().AddAsync(new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId
                    });
                await unitOfWork.SaveAsync();
            }


            return Unit.Value;

        }
    }
}
