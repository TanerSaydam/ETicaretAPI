using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.DTOs;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly IExternalAuthentication _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }       

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.GoogleLoginAsync(request.IdToken, 45);
            return new()
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Expiration = token.Expiration
            };
        }
    }
}
