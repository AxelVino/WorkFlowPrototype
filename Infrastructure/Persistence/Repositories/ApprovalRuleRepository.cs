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
        public async Task<List<ResponseApprovalRuleDto>> CompareApprovalRuleAsync(CompareDataQuery data)
        {
            var rules = await _dbContext.ApprovalRule.ToListAsync();

            var responseList = new List<ResponseApprovalRuleDto>();

            var stepOrders = rules.Select(r => r.StepOrder).Distinct().OrderBy(s => s).ToList();

            foreach (var stepOrder in stepOrders)
            {
                var matchedRules = rules
                    .Where(rule => rule.StepOrder == stepOrder)
                    .Where(rule =>
                        rule.MinAmount <= data.EstimatedAmount &&
                        (rule.MaxAmount == 0 || data.EstimatedAmount <= rule.MaxAmount) &&
                        (rule.Area == null || rule.Area == data.Area) &&
                        (rule.Type == null || rule.Type == data.Type)
                    )
                    .ToList();


                var bestMatch = matchedRules
                    .OrderByDescending(r => (r.Area.HasValue ? 1 : 0) + (r.Type.HasValue ? 1 : 0))
                    .ThenBy(r => r.MaxAmount == 0 ? decimal.MaxValue : r.MaxAmount)
                    .ThenByDescending(r => r.MinAmount)
                    .FirstOrDefault();

                if (bestMatch != null)
                {
                    responseList.Add(new ResponseApprovalRuleDto
                    {
                        StepOrder = bestMatch.StepOrder,
                        ApproverRoleId = bestMatch.ApproverRoleId,
                    });
                }
            }

            // Deduplicate by ApproverRoleId, keeping the one with the lowest StepOrder (earliest approval)
            var uniqueRoleList = responseList
                .GroupBy(r => r.ApproverRoleId)
                .Select(g => g.OrderBy(r => r.StepOrder).First())
                .OrderBy(r => r.StepOrder)
                .ToList();

            return uniqueRoleList;
        }
    }
}
