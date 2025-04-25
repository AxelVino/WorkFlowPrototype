using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using MediatR;

namespace Application.Services.ApprovalRuleService.ApprovalRuleQuerys
{
    public record CompareDataQuery(decimal EstimatedAmount, int Area, int Type) : IRequest<ResponseApprovalRuleDto>;
}
