using MediatR;

namespace Application.Services.ProposalService.ProposalQuerys
{
    public record GetAllTitleProjectInUse() : IRequest<List<string>>;
}
