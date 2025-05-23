using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class GetListApprovalStepsHandler : IRequestHandler<GetListApprovalStepsQuery, List<ProjectApprovalStep>>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public GetListApprovalStepsHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<List<ProjectApprovalStep>> Handle(GetListApprovalStepsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllFiltredAsync(request.request);
        }
    }
}
