using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ApprovalRuleService.ApprovalRuleQuerys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IApprovalRuleRepository
    {
        Task<List<ResponseApprovalRuleDto>> CompareApprovalRuleAsync(CompareDataQuery data);
    }
}
