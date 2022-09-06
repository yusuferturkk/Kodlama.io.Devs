using Application.Features.ProgrammingLanguages.Dtos;
using Application.Features.ProgrammingLanguages.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Persistence.Repositories;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Queries.GetByIdProgrammingLanguage
{
    public class GetByIdProgrammingLanguageQuery:IRequest<ProgrammingLanguageGetByIdDto>
    {
        public int Id { get; set; }

        public class GetByIdProgrammingLanguageQueryHandler:IRequestHandler<GetByIdProgrammingLanguageQuery, ProgrammingLanguageGetByIdDto>
        {
            private readonly IProgrammingLanguageRepository _repository;
            private readonly IMapper _mapper;
            private readonly ProgrammingLanguageBusinessRules _rules;

            public GetByIdProgrammingLanguageQueryHandler(IProgrammingLanguageRepository repository, IMapper mapper, 
                ProgrammingLanguageBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<ProgrammingLanguageGetByIdDto> Handle(GetByIdProgrammingLanguageQuery request, CancellationToken cancellationToken)
            {
                ProgrammingLanguage? programmingLanguage = await _repository.GetAsync(x => x.Id == request.Id);

                await _rules.ProgrammingLanguageShouldExistWhenRequested(programmingLanguage);

                ProgrammingLanguageGetByIdDto programmingLanguageGetByIdDto = _mapper.Map<ProgrammingLanguageGetByIdDto>(programmingLanguage);
                return programmingLanguageGetByIdDto;
            }
        }
    }
}
