using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public GoogleLoginCommandHandler(ITokenHandler tokenHandler, UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _tokenHandler = tokenHandler;
            _userManager = userManager;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "893161204353-nsv7ccqqcd386kt845ideqniarvdjrb5.apps.googleusercontent.com" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
                user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    NameSurname = payload.Name
                };
                var identityResult = await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
                result = identityResult.Succeeded;
            }

            //throw new Exception("Invalid external authentication");

            Token token = _tokenHandler.CreateAccessToken(5);

            return new()
            {
                AccessToken = token.AccessToken,
                Expiration = token.Expiration
            };
        }
    }
}
