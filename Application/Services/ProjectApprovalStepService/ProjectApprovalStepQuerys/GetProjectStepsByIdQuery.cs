using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys
{
    public record GetProjectStepsByIdQuery(Guid Id) : IRequest<List<ProjectApprovalStep>>;
}
