using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(50).MinimumLength(3).WithName("İsim Soyisim");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).MinimumLength(3).EmailAddress().WithName("Email");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithName("Şifre");
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(6).Equal(x=> x.Password).WithName("Tekrar Şifre");
        }
    }
}
