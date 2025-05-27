using Application.Interfaces.Repository;
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
    public class GetProposalProjectByIdHandler : IRequestHandler<GetProposalProjectByIdQuery, ProjectProposal>
    {
        private readonly IProjectProposalRepository _repository;

        public GetProposalProjectByIdHandler(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectProposal> Handle(GetProposalProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProjectProposalById(request.Id);

        }
    }
}
