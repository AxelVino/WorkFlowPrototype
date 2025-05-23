using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalQuerys
{
    public record GetAllProposalProjectsQuery(ProposalFilterRequest request) : IRequest<List<ProjectProposal>>;
}
