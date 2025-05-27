using Domain.Entities;
using MediatR;


namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands
{
    public class CreateProjectApprovalSteps() : IRequest<List<ProjectApprovalStep>>
    {
        public required List<ProjectApprovalStep> Steps { get; set; }
    }
}
