using OnionArcAndAll.Application.Bases;
using OnionArcAndAll.Application.Features.Auth.Exceptions;
using OnionArcAndAll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Rules
{
    public class AuthRules : BaseRules
    {
        public Task UserShouldNotBeExist(User? user)
        {
            if (user is not null) throw new UserAlreadyExistException();

            return Task.CompletedTask;
        }
        public Task EmailOrPasswordShouldNotBeInvalid(User? user, bool checkPassword)
        {
            if (user is null || !checkPassword) throw new EmailOrPasswordShouldNotBeInvalidException();

            return Task.CompletedTask;
        }
        public Task CheckRefreshTokenExpiryTime(DateTime? expiryTime)
        {
            if (expiryTime <= DateTime.Now)
                throw new RefreshTokenExpiryTimeException();

            return Task.CompletedTask;
        }
        public Task CheckEmailAddressValid(User? user)
        {
            if (user is null)
                throw new CheckEmailAddressValidException();

            return Task.CompletedTask;
        }



    }
}
