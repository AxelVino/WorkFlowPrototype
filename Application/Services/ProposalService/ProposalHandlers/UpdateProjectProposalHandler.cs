using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalCommands;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalHandlers
{
    public class UpdateProjectProposalHandler : IRequestHandler<UpdateProjectProposalCommand, ProjectProposal>
    {
        private readonly IProjectProposalRepository _repository;

        public UpdateProjectProposalHandler(IProjectProposalRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProjectProposal> Handle(UpdateProjectProposalCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateProposalAsync(request.Request);
        }
    }   
}
