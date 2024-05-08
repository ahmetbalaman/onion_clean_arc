using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Features.Auth.Command.Logout
{
    public class RevokeCommandRequest : IRequest<Unit>
    {
        public string Email { get; set; }

    }
}
