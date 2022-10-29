using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommanResponse
    {
            
    }

    public class LoginUserSuccessCommandResponse : LoginUserCommanResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }

    }

    public class LoginUserErrorCommandResponse : LoginUserCommanResponse
    {
        public string Message { get; set; }
    }
}
