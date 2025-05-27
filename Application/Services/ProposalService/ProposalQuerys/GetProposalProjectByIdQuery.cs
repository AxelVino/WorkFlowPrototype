using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalQuerys
{
    public record GetProposalProjectByIdQuery(Guid Id) : IRequest<ProjectProposal>;
}
