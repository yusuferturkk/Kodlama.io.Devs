using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Commands.CreateOperationClaim
{
    public class CreateOperationClaimCommand : IRequest<CreatedOperationClaimDto>
    {
        public string Name { get; set; }

        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreatedOperationClaimDto>
        {
            private readonly IOperationClaimRepository _repository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _rules;

            public CreateOperationClaimCommandHandler(IOperationClaimRepository repository, IMapper mapper, OperationClaimBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<CreatedOperationClaimDto> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {                
                await _rules.OperationClaimCanNotBeDuplicated(request.Name);

                var mappedOperationClaim = _mapper.Map<OperationClaim>(request);
                var createdOperationClaim = await _repository.AddAsync(mappedOperationClaim);
                var createdOperationClaimDto = _mapper.Map<CreatedOperationClaimDto>(createdOperationClaim);
                return createdOperationClaimDto;
            }
        }
    }
}
