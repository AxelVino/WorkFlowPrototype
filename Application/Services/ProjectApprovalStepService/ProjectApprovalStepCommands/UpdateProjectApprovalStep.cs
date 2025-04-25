using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands
{
    public record UpdateProjectApprovalStep(ProjectApprovalStep Project) : IRequest<bool>;
}
