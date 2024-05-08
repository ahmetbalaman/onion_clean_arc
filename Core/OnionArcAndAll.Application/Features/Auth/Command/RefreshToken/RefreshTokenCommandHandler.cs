using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Features.Auth.Rules;
using OnionArcAndAll.Application.Interfaces.AutoMapper;
using OnionArcAndAll.Application.Interfaces.Tokens;
using OnionArcAndAll.Application.Interfaces.UnitOfWorks;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.RefreshToken
{
    internal class RefreshTokenCommandHandler : BaseHandler,IRequestHandler<RefreshTokenCommandRequest
        , RefreshTokenCommandResponse>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly AuthRules authRules;
        

        public RefreshTokenCommandHandler(
            AuthRules authRules,
        UserManager<User> userManager, ITokenService tokenService,
            IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.authRules = authRules;
        }

        public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal? principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            string? email = principal!.FindFirstValue(ClaimTypes.Email);

            User? user = await userManager.FindByEmailAsync(email!);
            IList<string> roles = await userManager.GetRolesAsync(user!);

            _ = authRules.CheckRefreshTokenExpiryTime(user!.RefreshTokenExpiryTime);

            JwtSecurityToken newAccessToken = await tokenService.CreateToken(user, roles);
            string newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);


            return new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }
    }
}
