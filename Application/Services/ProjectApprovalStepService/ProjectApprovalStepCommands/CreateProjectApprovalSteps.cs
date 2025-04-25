using Domain.Entities;
using MediatR;


namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands
{
    public class CreateProjectApprovalSteps() : IRequest<bool>
    {
        public required List<ProjectApprovalStep> Steps { get; set; }
    }
}
