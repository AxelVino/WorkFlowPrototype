using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepCommands;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using MediatR;

namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class VerifyStepsHandler : IRequestHandler<VerifyStepsQuery, bool>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public VerifyStepsHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<bool> Handle(VerifyStepsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.VerifyStepsSameGuid(request.Guid);
        }
    }
}
