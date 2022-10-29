using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CeateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommanRequest, CreateUserCommandReponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandReponse> Handle(CreateUserCommanRequest request, CancellationToken cancellationToken)
        {
           IdentityResult result = await _userManager.CreateAsync(new()
            {
               Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                NameSurname = request.NameSurName
            }, request.Password);

            CreateUserCommandReponse commandReponse = new() { Succeeded = result.Succeeded};

            if (result.Succeeded)
                commandReponse.Message = "Kullanıcı başarıyla oluşturuldu";
            else            
                foreach (var error in result.Errors)                
                    commandReponse.Message += $"{error.Code} - {error.Description}<br>";
                
            

            return commandReponse;
            //throw new UserCreateFaildException();


        }
    }
}
