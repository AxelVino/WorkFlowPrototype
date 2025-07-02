using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalCommands
{
    public record UpdateProjectProposalCommand(ProjectProposal Request) : IRequest<ProjectProposal>;
}
