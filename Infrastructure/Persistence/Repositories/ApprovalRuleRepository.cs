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

                if (!matchedRules.Any())
                {
                    break;
                }

                foreach (var rule in matchedRules)
                {
                    responseList.Add(new ResponseApprovalRuleDto
                    {
                        StepOrder = rule.StepOrder,
                        ApproverRoleId = rule.ApproverRoleId,
                    });
                }
            }
            return responseList;
        }
    }
}
