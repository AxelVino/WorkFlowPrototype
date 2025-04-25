using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalCommands
{
    public record UpdateProjectProposalCommand(ProjectProposal proyect) : IRequest<bool>;
}
