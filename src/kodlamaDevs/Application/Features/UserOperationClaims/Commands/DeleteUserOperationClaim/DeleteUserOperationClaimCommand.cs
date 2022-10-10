using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim
{
    public class DeleteUserOperationClaimCommand : IRequest<DeletedUserOperationClaimDto>
    {
        public int Id { get; set; }

        public class DeleteUserOperationClaimCommandHandler : IRequestHandler<DeleteUserOperationClaimCommand, DeletedUserOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _repository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _rules;

            public DeleteUserOperationClaimCommandHandler(IUserOperationClaimRepository repository, IMapper mapper, UserOperationClaimBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<DeletedUserOperationClaimDto> Handle(DeleteUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                var userOperationClaim = await _repository.GetAsync(u => u.Id == request.Id);

                var deleteUserOperationClaim = await _repository.DeleteAsync(userOperationClaim);
                var deletedUserOperationClaimDto = _mapper.Map<DeletedUserOperationClaimDto>(deleteUserOperationClaim);
                return deletedUserOperationClaimDto;
            }
        }
    }
}
