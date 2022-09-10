using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubProfiles.Rules
{
    public class GithubProfileBusinessRules
    {
        private readonly IGithubProfileRepository _repository;

        public GithubProfileBusinessRules(IGithubProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task GithubProfileUrlCanNotBeDuplicatedWhenInserted(string url)
        {
            IPaginate<GithubProfile> result = await _repository.GetListAsync(t => t.ProfileUrl == url);
            if (result.Items.Any()) throw new BusinessException("Github profile url exists.");
        }

        public async Task GithubProfileShouldExistWhenRequested(GithubProfile githubProfile)
        {
            if (githubProfile == null) throw new BusinessException("Requested user does not exist.");
        }
    }
}
