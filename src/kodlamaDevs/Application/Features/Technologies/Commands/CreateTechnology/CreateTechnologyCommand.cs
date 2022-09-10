using Application.Features.Technologies.Dtos;
using Application.Features.Technologies.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Technologies.Commands.CreateTechnology
{
    public class CreateTechnologyCommand : IRequest<CreatedTechnologyDto>
    {
        public int ProgrammingLanguageId { get; set; }
        public string Name { get; set; }

        public class CreateTechnologyCommandHandler : IRequestHandler<CreateTechnologyCommand, CreatedTechnologyDto>
        {
            private readonly ITechnologyRepository _repository;
            private readonly IMapper _mapper;
            private readonly TechnologyBusinessRules _rules;

            public CreateTechnologyCommandHandler(ITechnologyRepository repository, IMapper mapper, TechnologyBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<CreatedTechnologyDto> Handle(CreateTechnologyCommand request, CancellationToken cancellationToken)
            {
                Technology? technology = await _repository.GetAsync(c => c.ProgrammingLanguageId == request.ProgrammingLanguageId);

                await _rules.TechnologyShouldExistWhenRequested(technology);
                await _rules.TechnologyNameCanNotBeDuplicatedWhenInserted(request.Name);

                Technology mappedTechnology = _mapper.Map<Technology>(request);
                Technology createdTechnology = await _repository.AddAsync(mappedTechnology);
                CreatedTechnologyDto result = _mapper.Map<CreatedTechnologyDto>(createdTechnology);
                return result;
            }
        }
    }
}
