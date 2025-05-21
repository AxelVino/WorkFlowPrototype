using Application.Interfaces.Repository;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Microsoft.EntityFrameworkCore;
using Application.Services.ApprovalRuleService.ApprovalRuleQuerys;

namespace Infrastructure.Persistence.Repositories
{
    public class ApprovalRuleRepository : IApprovalRuleRepository
    {
        private readonly DbContextApprover _dbContext;

        public ApprovalRuleRepository(DbContextApprover dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResponseApprovalRuleDto> CompareApprovalRuleAsync(CompareDataQuery data)
        {
            var rules = await _dbContext.ApprovalRule.ToListAsync();

            var bestMatch = rules
            .Where(rule =>
                    rule.MinAmount <= data.EstimatedAmount &&
                    (rule.MaxAmount == 0 || data.EstimatedAmount <= rule.MaxAmount) &&
                    (rule.Area == null || rule.Area == data.Area) &&
                    (rule.Type == null || rule.Type == data.Type)
                )
                .Select(rule => new
                {
                    Rule = rule,
                    Specificity =
                        (rule.Area != null ? 1 : 0) +
                        (rule.Type != null ? 1 : 0) +
                        (rule.MinAmount > 0 ? 1 : 0) +
                        (rule.MaxAmount > 0 ? 1 : 0)
                })
                .OrderByDescending(x => x.Specificity)
                .ThenByDescending(x => x.Rule.MaxAmount != 0)
                .FirstOrDefault();

            return new ResponseApprovalRuleDto
            {
                ApproverRoleId = bestMatch.Rule.ApproverRoleId,
                StepOrder = bestMatch.Rule.StepOrder
            };
        }
    }
}
