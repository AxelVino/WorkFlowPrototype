using Application.Services.ApprovalRuleService.ApprovalRuleDto;

namespace Application.Interfaces.ApprovalRule
{
    public interface IApprovalRuleService
    {
        Task<ResponseApprovalRuleDto> MatchProposalWithRuleAsync(decimal estimatedAmount, int Area, int Type);
    }
}
