using Application.Features.Users.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.UserRegister
{
    public class UserRegisterCommand : UserForRegisterDto, IRequest<AccessToken>
    {
        public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, AccessToken>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;
            private readonly ITokenHelper _tokenHelper;
            private readonly UserBusinessRules _rules;

            public UserRegisterCommandHandler(IUserRepository repository, IMapper mapper, ITokenHelper tokenHelper, UserBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _tokenHelper = tokenHelper;
                _rules = rules;
            }

            public async Task<AccessToken> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };

                await _rules.EmailCanNotBeDuplicatedWhenInserted(user.Email);
                User newUser = await _repository.AddAsync(user);
                var token = _tokenHelper.CreateToken(newUser, new List<OperationClaim>());
                return token;
            }
        }
    }
}
