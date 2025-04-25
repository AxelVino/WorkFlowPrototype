using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalCommands;
using Application.Services.ProposalService.ProposalQuerys;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ProposalService.ProposalHandlers
{
    public class GetAllProposalByUserHandler : IRequestHandler< GetAllProposalByUserQuery, List<ProjectProposal>>
    {
        private readonly IProjectProposalRepository _repository;

        public GetAllProposalByUserHandler(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProjectProposal>> Handle(GetAllProposalByUserQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProposalByUserAsync(request.IdUser);

        }
    }
}
