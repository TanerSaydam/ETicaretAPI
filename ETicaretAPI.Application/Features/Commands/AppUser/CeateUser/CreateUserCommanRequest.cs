using ETicaretAPI.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CeateUser
{
    public class CreateUserCommanRequest : CreateUser, IRequest<CreateUserCommandReponse>
    {
        
    }
}
