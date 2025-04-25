using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectApprovalStep, bool>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public UpdateProjectHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<bool> Handle(UpdateProjectApprovalStep request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateProject(request.Project);
        }
    }
}
