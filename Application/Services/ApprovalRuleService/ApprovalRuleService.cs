using Application.Interfaces.ApprovalRule;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ApprovalRuleService.ApprovalRuleQuerys;
using MediatR;

namespace Application.Services.ApprovalRuleService
{
    public class ApprovalRuleService : IApprovalRuleService
    {
        private readonly IMediator _mediator;
        public ApprovalRuleService(IMediator mediator) {

            _mediator = mediator;
        }

        public async Task<List<ResponseApprovalRuleDto>> MatchProposalWithRuleAsync(decimal estimatedAmount, int Area, int Type)
        {
            return await _mediator.Send(new CompareDataQuery(estimatedAmount, Area, Type));
        }
    }
}
