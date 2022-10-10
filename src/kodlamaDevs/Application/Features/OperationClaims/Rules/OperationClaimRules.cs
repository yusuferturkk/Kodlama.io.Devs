using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules
    {
        private readonly IOperationClaimRepository _repository;

        public OperationClaimBusinessRules(IOperationClaimRepository repository)
        {
            _repository = repository;
        }

        public async Task OperationClaimCanNotBeDuplicated(string name)
        {
            IPaginate<OperationClaim> result = await _repository.GetListAsync(o => o.Name == name);
            if (result.Items.Any()) throw new BusinessException("Claim already exists");
        }

        public async Task OperationClaimShouldExistWhenRequested(OperationClaim operationClaim)
        {
            if (operationClaim == null) throw new BusinessException("Claim is not exists");
        }
    }
}
