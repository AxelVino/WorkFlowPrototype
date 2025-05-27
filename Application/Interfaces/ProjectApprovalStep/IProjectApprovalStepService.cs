using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Application.Services.ProposalService.ProposalDtos;

namespace Application.Interfaces.ProjectApprovalStep
{
    public interface IProjectApprovalStepService
    {
        Task<List<ProjectStepResponse>> CreateProjectApprovalStepAsync(List<ResponseApprovalRuleDto> rules, Domain.Entities.ProjectProposal proposal);
        Task<List<ProjectProposalResponse>> GetAllProjectsFiltred(ProposalFilterRequest request);
        Task<List<ProjectStepResponse>> GetProjectStepsById(Guid id);
        Task<ProposalResponse> DecideStatus(Guid id, DecisionRequest request);
    }
}
