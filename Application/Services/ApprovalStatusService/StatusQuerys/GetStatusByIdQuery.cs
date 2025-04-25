using MediatR;


namespace Application.Services.ApprovalStatusService.StatusQuerys
{
    public record GetStatusByIdQuery(int Id) : IRequest<Domain.Entities.ApprovalStatus>;
}
