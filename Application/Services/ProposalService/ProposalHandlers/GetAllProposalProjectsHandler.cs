using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalHandlers
{
    public class GetAllProposalProjectsHandler : IRequestHandler<GetAllProposalProjectsQuery, List<ProjectProposal>>
    {
        private readonly IProjectProposalRepository _repository;

        public GetAllProposalProjectsHandler(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProjectProposal>> Handle(GetAllProposalProjectsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProposalProjects(request.request);
        }
    }
}
