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

namespace Application.Features.UserOperationClaims.Commands.UpdateUserOperationClaim
{
    public class UpdateUserOperationClaimCommand : IRequest<UpdatedUserOperationClaimDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }

        public class UpdateUserOperationClaimCommandHandler : IRequestHandler<UpdateUserOperationClaimCommand, UpdatedUserOperationClaimDto>
        {
            private readonly IUserOperationClaimRepository _repository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _rules;

            public UpdateUserOperationClaimCommandHandler(IUserOperationClaimRepository repository, IMapper mapper, UserOperationClaimBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<UpdatedUserOperationClaimDto> Handle(UpdateUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                var userOperationClaim = await _repository.GetAsync(u => u.Id == request.Id);
                userOperationClaim.UserId = request.UserId;
                userOperationClaim.OperationClaimId = request.OperationClaimId;

                var updateUserOperationClaim = await _repository.UpdateAsync(userOperationClaim);
                var updatedUserOperationClaimDto = _mapper.Map<UpdatedUserOperationClaimDto>(updateUserOperationClaim);
                return updatedUserOperationClaimDto;
            }
        }
    }
}
