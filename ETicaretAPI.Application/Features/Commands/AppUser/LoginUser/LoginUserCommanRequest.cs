using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommanRequest : IRequest<LoginUserCommanResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
