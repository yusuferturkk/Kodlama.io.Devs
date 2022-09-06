using Application.Features.ProgrammingLanguages.Commands.CreateProgrammingLanguage;
using Application.Features.ProgrammingLanguages.Dtos;
using Application.Features.ProgrammingLanguages.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProgrammingLanguages.Commands.UpdateProgrammingLanguage
{
    public class UpdateProgrammingLanguageCommand : IRequest<UpdatedProgrammingLanguageDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateProgrammingLanguageCommandHandler : IRequestHandler<UpdateProgrammingLanguageCommand, UpdatedProgrammingLanguageDto>
        {
            private readonly IProgrammingLanguageRepository _repository;
            private readonly IMapper _mapper;
            private readonly ProgrammingLanguageBusinessRules _rules;

            public UpdateProgrammingLanguageCommandHandler(IProgrammingLanguageRepository repository, IMapper mapper, ProgrammingLanguageBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<UpdatedProgrammingLanguageDto> Handle(UpdateProgrammingLanguageCommand request, CancellationToken cancellationToken)
            {
                ProgrammingLanguage? programmingLanguage = await _repository.GetAsync(x => x.Id == request.Id);

                await _rules.ProgrammingLanguageShouldExistWhenRequested(programmingLanguage);
                await _rules.ProgrammingLanguageNameCanNotDuplicatedWhenInserted(request.Name);

                programmingLanguage.Name = request.Name;

                ProgrammingLanguage updatedProgrammingLanguage = await _repository.UpdateAsync(programmingLanguage);
                UpdatedProgrammingLanguageDto updatedProgrammingLanguageDto = _mapper.Map<UpdatedProgrammingLanguageDto>(updatedProgrammingLanguage);
                

                return updatedProgrammingLanguageDto;
            }
        }
    }
}
