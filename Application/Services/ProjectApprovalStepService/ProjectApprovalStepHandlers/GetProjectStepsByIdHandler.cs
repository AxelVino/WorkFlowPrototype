using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class GetProjectStepsByIdHandler : IRequestHandler<GetProjectStepsByIdQuery, List<ProjectApprovalStep>>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public GetProjectStepsByIdHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<List<ProjectApprovalStep>> Handle(GetProjectStepsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProjectStepsById(request.Id);
        }
    }
}
