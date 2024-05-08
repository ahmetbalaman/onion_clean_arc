using OnionArcAndAll.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Exceptions
{
    public class CheckEmailAddressValidException : BaseException
    {
        public CheckEmailAddressValidException() : base("Böyle bir email adresinden hesap bulunamamaktadır")
        {
        }
    }
}
