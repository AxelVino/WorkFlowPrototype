using Application.Interfaces.Repository;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ApprovalRuleService.ApprovalRuleQuerys;
using MediatR;

namespace Application.Services.ApprovalRuleService.ApprovalRuleHandlers
{
    public class CompareApprovalRuleHandler : IRequestHandler<CompareDataQuery, ResponseApprovalRuleDto>
    {
        private readonly IApprovalRuleRepository _repository;

        public CompareApprovalRuleHandler(IApprovalRuleRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<ResponseApprovalRuleDto> Handle(CompareDataQuery request, CancellationToken cancellationToken)
        {
            return await _repository.CompareApprovalRuleAsync(request);
        }
    }
}

