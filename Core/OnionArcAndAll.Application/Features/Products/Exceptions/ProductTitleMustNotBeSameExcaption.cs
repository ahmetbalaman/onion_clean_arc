using OnionArcAndAll.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Products.Exceptions
{
    public class ProductTitleMustNotBeSameExcaption : BaseException
    {
        public ProductTitleMustNotBeSameExcaption() :base("Ürün başlığı zaten var")
        {
            
        }
    }
}
