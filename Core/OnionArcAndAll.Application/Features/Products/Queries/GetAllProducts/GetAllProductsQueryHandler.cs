using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArcAndAll.Application.DTOs;
using OnionArcAndAll.Application.Interfaces.AutoMapper;
using OnionArcAndAll.Application.Interfaces.UnitOfWorks;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.GetReadRepository<Product>().GetAllAsync(include: x=> x.Include(b=> b.Brand));


            var brandMap = mapper.Map<BrandDTO, Brand>(new Brand());

            var map = mapper.Map<GetAllProductsQueryResponse, Product>(products);

            foreach (var item in map)
                item.Price -= (item.Price * item.Discount / 100);

           return map;

        }
    }
}
