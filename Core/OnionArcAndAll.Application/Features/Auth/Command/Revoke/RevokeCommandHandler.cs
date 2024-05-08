using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Features.Auth.Rules;
using OnionArcAndAll.Application.Interfaces.AutoMapper;
using OnionArcAndAll.Application.Interfaces.UnitOfWorks;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.Logout
{
    public class RevokeCommandHandler : BaseHandler, IRequestHandler<
        RevokeCommandRequest,
        Unit
        >
    {
        private readonly AuthRules authRules;
        private readonly UserManager<User> userManager;

        public RevokeCommandHandler(AuthRules authRules, UserManager<User> userManager, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(RevokeCommandRequest request, CancellationToken cancellationToken)
        {
            User? user = await userManager.FindByEmailAsync(request.Email);

            await authRules.CheckEmailAddressValid(user);

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await userManager.UpdateAsync(user);
            return Unit.Value;
        }
    }
}
