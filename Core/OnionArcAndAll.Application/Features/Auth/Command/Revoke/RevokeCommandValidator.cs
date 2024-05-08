using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.Logout
{
    public class RevokeCommandValidator : AbstractValidator<RevokeCommandRequest>
    {
       public RevokeCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().EmailAddress();
        }   
    }
}
