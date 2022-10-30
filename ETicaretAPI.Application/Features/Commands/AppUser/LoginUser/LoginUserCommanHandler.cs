using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommanHandler : IRequestHandler<LoginUserCommanRequest, LoginUserCommanResponse>
    {
        private readonly IInternalAuthentication _authService;

        public LoginUserCommanHandler(IInternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommanResponse> Handle(LoginUserCommanRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 45);

            return new LoginUserSuccessCommandResponse()
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Expiration = token.Expiration
            };            
        }
    }
}
