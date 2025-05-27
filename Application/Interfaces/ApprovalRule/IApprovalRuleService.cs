using Application.Services.ApprovalRuleService.ApprovalRuleDto;

namespace Application.Interfaces.ApprovalRule
{
    public interface IApprovalRuleService
    {
        Task<List<ResponseApprovalRuleDto>> MatchProposalWithRuleAsync(decimal estimatedAmount, int Area, int Type);
    }
}
