using Application.Features.GithubProfiles.Dtos;
using Application.Features.GithubProfiles.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubProfiles.Commands.UpdateGithubProfile
{
    public class UpdateGithubProfileCommand : IRequest<UpdatedGithubProfileDto>
    {
        public int UserId { get; set; }
        public string ProfileUrl { get; set; }

        public class UpdateGithubProfileCommandHandler : IRequestHandler<UpdateGithubProfileCommand, UpdatedGithubProfileDto>
        {
            private readonly IGithubProfileRepository _repository;
            private readonly IMapper _mapper;
            private readonly GithubProfileBusinessRules _rules;

            public UpdateGithubProfileCommandHandler(IGithubProfileRepository repository, IMapper mapper, GithubProfileBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<UpdatedGithubProfileDto> Handle(UpdateGithubProfileCommand request, CancellationToken cancellationToken)
            {
                GithubProfile? githubProfile = await _repository.GetAsync(c => c.UserId == request.UserId);
                githubProfile.ProfileUrl = request.ProfileUrl;

                await _rules.GithubProfileShouldExistWhenRequested(githubProfile);
                await _rules.GithubProfileUrlCanNotBeDuplicatedWhenInserted(request.ProfileUrl);

                GithubProfile updatedGithubProfile = await _repository.UpdateAsync(githubProfile);
                UpdatedGithubProfileDto result = _mapper.Map<UpdatedGithubProfileDto>(updatedGithubProfile);
                return result;
            }
        }
    }
}
