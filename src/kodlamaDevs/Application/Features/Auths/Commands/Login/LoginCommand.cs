using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auths.Commands.Login
{
    public class LoginCommand : IRequest<LoggedInDto>
    {
        public UserForLoginDto UserForLoginDto { get; set; }
        public string IpAddress { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInDto>
        {
            private readonly IUserRepository _repository;
            private readonly IAuthService _authService;
            private readonly AuthBusinessRules _rules;

            public LoginCommandHandler(IUserRepository repository, IAuthService authService, AuthBusinessRules rules)
            {
                _repository = repository;
                _authService = authService;
                _rules = rules;
            }

            public async Task<LoggedInDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                User? user = await _repository.GetAsync(u => u.Email == request.UserForLoginDto.Email);

                _rules.CheckIfUserExists(user);
                _rules.CheckIfPasswordIsCorrect(request.UserForLoginDto.Password, user.PasswordHash, user.PasswordSalt);

                AccessToken createdAccessToken = await _authService.CreateAccessToken(user);
                RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IpAddress);
                RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

                LoggedInDto loggedInDto = new()
                {
                    AccessToken = createdAccessToken,
                    RefreshToken = createdRefreshToken,
                };

                return loggedInDto;
            }
        }
    }
}
