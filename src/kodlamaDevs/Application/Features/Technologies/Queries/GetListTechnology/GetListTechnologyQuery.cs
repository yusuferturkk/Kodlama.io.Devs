using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Models;
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

namespace Application.Features.Technologies.Queries.GetListTechnology
{
    public class GetListTechnologyQuery : IRequest<TechnologyListModel>
    {
        public PageRequest PageRequest { get; set; }

        public class GetListTechnologyQueryHandler : IRequestHandler<GetListTechnologyQuery, TechnologyListModel>
        {
            private readonly ITechnologyRepository _repository;
            private readonly IMapper _mapper;

            public GetListTechnologyQueryHandler(ITechnologyRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<TechnologyListModel> Handle(GetListTechnologyQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Technology> technology = await _repository.GetListAsync(
                    include: m => m.Include(c => c.ProgrammingLanguage),
                    index: request.PageRequest.Page, size: request.PageRequest.PageSize);
                TechnologyListModel model = _mapper.Map<TechnologyListModel>(technology);
                return model;
            }
        }
    }
}
