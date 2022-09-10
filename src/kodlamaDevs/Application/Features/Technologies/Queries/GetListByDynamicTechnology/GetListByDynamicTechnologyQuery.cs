using Application.Features.Technologies.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Technologies.Queries.GetListByDynamicTechnology
{
    public class GetListByDynamicTechnologyQuery : IRequest<TechnologyListModel>
    {
        public Dynamic Dynamic  { get; set; }
        public PageRequest PageRequest { get; set; }

        public class GetListByDynamicTechnologyQueryHandler : IRequestHandler<GetListByDynamicTechnologyQuery, TechnologyListModel>
        {
            private readonly ITechnologyRepository _repository;
            private readonly IMapper _mapper;

            public GetListByDynamicTechnologyQueryHandler(ITechnologyRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<TechnologyListModel> Handle(GetListByDynamicTechnologyQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Technology> technology = await _repository.GetListByDynamicAsync(
                    dynamic: request.Dynamic, include: m => m.Include(c => c.ProgrammingLanguage),
                    index: request.PageRequest.Page, size: request.PageRequest.PageSize);
                TechnologyListModel model = _mapper.Map<TechnologyListModel>(technology);
                return model;
            }
        }
    }
}
