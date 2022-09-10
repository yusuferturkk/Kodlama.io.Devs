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

namespace Application.Features.GithubProfiles.Commands.CreateGithubProfile
{
    public class CreateGithubProfileCommand : IRequest<CreatedGithubProfileDto>
    {
        public int UserId { get; set; }
        public string ProfileUrl { get; set; }

        public class CreateGithubProfileCommandHandler : IRequestHandler<CreateGithubProfileCommand, CreatedGithubProfileDto>
        {
            private readonly IGithubProfileRepository _repository;
            private readonly IMapper _mapper;
            private readonly GithubProfileBusinessRules _rules;

            public CreateGithubProfileCommandHandler(IGithubProfileRepository repository, IMapper mapper, GithubProfileBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<CreatedGithubProfileDto> Handle(CreateGithubProfileCommand request, CancellationToken cancellationToken)
            {
                GithubProfile? githubProfile = await _repository.GetAsync(c => c.UserId == request.UserId);

                await _rules.GithubProfileShouldExistWhenRequested(githubProfile);
                await _rules.GithubProfileUrlCanNotBeDuplicatedWhenInserted(request.ProfileUrl);

                GithubProfile mappedGithubProfile = _mapper.Map<GithubProfile>(request);
                GithubProfile createdGithubProfile = await _repository.AddAsync(mappedGithubProfile);
                CreatedGithubProfileDto result = _mapper.Map<CreatedGithubProfileDto>(createdGithubProfile);
                return result;
            }
        }
    }
}
