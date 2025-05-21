using MediatR;

namespace Application.Services.ApprovalStatusService.StatusQuerys
{
    public record GetAllApprovalStatusQuery() : IRequest<List<Domain.Entities.ApprovalStatus>>;

}
