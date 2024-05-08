using OnionArcAndAll.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Exceptions
{
    public class RefreshTokenExpiryTimeException : BaseException
    {
        public RefreshTokenExpiryTimeException() : base("Oturum süresi sona ermiştir. Lütfen tekrar giriş yapın")
        {
        }
    }
}
