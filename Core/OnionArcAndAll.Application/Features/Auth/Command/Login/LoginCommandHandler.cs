using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.Login
{
    public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly AuthRules authRules;
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

   

        public LoginCommandHandler(IConfiguration configuration, 
            ITokenService tokenService, IMapper mapper, IUnitOfWork
            unitOfWork, IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.userManager = userManager;
            this.authRules = authRules;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
           User user = await userManager.FindByEmailAsync(request.Email);
            bool checkPassword = await userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
            {
                throw new Exception("Invalid Password");
            }

            await authRules.EmailOrPasswordShouldNotBeInvalid(user, checkPassword);
            IList<string> roles = await userManager.GetRolesAsync(user);
            JwtSecurityToken token = await tokenService.CreateToken(user, roles);

            string refreshToken = tokenService.GenerateRefreshToken();

            _ = int.TryParse(configuration["JWT:RefreshTokenValidtyInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays); 

            await userManager.UpdateAsync(user);
            await userManager.UpdateSecurityStampAsync(user);


            string _token = new JwtSecurityTokenHandler().WriteToken(token);

            await userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", _token);


            return new()
            {
                Token = _token,
                RefreshToken = refreshToken,
                TokenExpiration = token.ValidTo
            };
        }
    }
}
