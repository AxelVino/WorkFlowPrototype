using Application.Responses;
using Application.Services.ApprovalRuleService.ApprovalRuleDto;
using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;

namespace Application.Interfaces.ProjectApprovalStep
{
    public interface IProjectApprovalStepService
    {
        Task<List<ApprovalStep>> CreateProjectApprovalStepAsync(List<ResponseApprovalRuleDto> rules, Domain.Entities.ProjectProposal proposal);
        Task<List<ProjectProposalResponse>> GetAllProjectsFiltred(ProposalFilterRequest request);
        Task<List<ApprovalStep>> GetProjectStepsById(Guid id);
        Task<List<Domain.Entities.ProjectApprovalStep>> GetStepById(Guid id);
    }
}
