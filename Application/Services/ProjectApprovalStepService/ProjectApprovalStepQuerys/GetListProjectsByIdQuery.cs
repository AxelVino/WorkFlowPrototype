using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys
{
    public record GetListProjectsByIdQuery(int approverRoleId) : IRequest<List<ProjectApprovalStep>>;
}
