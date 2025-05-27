using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectApprovalSteps, List<ProjectApprovalStep>>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public CreateProjectHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<List<ProjectApprovalStep>> Handle(CreateProjectApprovalSteps request, CancellationToken cancellationToken)
        {
            return await _repository.AddRangeAsync(request.Steps);
        }
    }
}
