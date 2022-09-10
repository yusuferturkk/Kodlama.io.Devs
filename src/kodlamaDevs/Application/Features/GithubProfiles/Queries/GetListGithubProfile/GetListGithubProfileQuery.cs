using Application.Features.GithubProfiles.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubProfiles.Queries.GetListGithubProfile
{
    public class GetListGithubProfileQuery : IRequest<GithubProfileListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListGithubProfileQueryHandler : IRequestHandler<GetListGithubProfileQuery, GithubProfileListModel>
        {
            private readonly IGithubProfileRepository _repository;
            private readonly IMapper _mapper;

            public GetListGithubProfileQueryHandler(IGithubProfileRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<GithubProfileListModel> Handle(GetListGithubProfileQuery request, CancellationToken cancellationToken)
            {
                IPaginate<GithubProfile> githubProfile = await _repository.GetListAsync(
                    include: m => m.Include(m => m.User),
                    index: request.PageRequest.Page, size: request.PageRequest.PageSize);
                GithubProfileListModel model = _mapper.Map<GithubProfileListModel>(githubProfile);
                return model;
            }
        }
    }
}
