using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Features.Products.Exceptions;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Products.Rules
{
    public class ProductRules : BaseRules
    {
        public Task ValidateProductTitle(IList<Product> products, string requestTitle)
        {
            if (products.Any(x => x.Title == requestTitle))
                throw new ProductTitleMustNotBeSameExcaption();
           
            return Task.CompletedTask;
        }
    }
}
