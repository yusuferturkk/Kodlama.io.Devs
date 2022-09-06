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

namespace Application.Features.ProgrammingLanguages.Commands.DeleteProgrammingLanguage
{
    public class DeleteProgrammingLanguageCommand : IRequest<DeletedProgrammingLanguageDto>
    {
        public int Id { get; set; }

        public class DeleteProgrammingLanguageCommandHandler : IRequestHandler<DeleteProgrammingLanguageCommand, DeletedProgrammingLanguageDto>
        {
            private readonly IProgrammingLanguageRepository _repository;
            private readonly IMapper _mapper;
            private readonly ProgrammingLanguageBusinessRules _rules;

            public DeleteProgrammingLanguageCommandHandler(IProgrammingLanguageRepository repository, IMapper mapper, 
                                                        ProgrammingLanguageBusinessRules rules)
            {
                _repository = repository;
                _mapper = mapper;
                _rules = rules;
            }

            public async Task<DeletedProgrammingLanguageDto> Handle(DeleteProgrammingLanguageCommand request, CancellationToken cancellationToken)
            {
                ProgrammingLanguage? programmingLanguage = await _repository.GetAsync(x => x.Id == request.Id);

                await _rules.ProgrammingLanguageShouldExistWhenRequested(programmingLanguage);

                ProgrammingLanguage deletedProgrammingLanguage = await _repository.DeleteAsync(programmingLanguage);
                DeletedProgrammingLanguageDto deletedProgrammingLanguageDto = _mapper.Map<DeletedProgrammingLanguageDto>(deletedProgrammingLanguage);

                return deletedProgrammingLanguageDto;
            }
        }
    }
}
