using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Application.Services.ProposalService.ProposalDtos;

namespace Application.Interfaces.ProjectApprovalStep
{
    public interface IProjectApprovalStepService
    {
        Task<bool>CreateProjectApprovalStepAsync(IncompletedProjectDto incompleted);
        Task<List<ProjectProposalResponse>> GetAllProjectsFiltred(ProposalFilterRequest request); 
        Task<bool>ApproveProjectStepAsync(Domain.Entities.ProjectApprovalStep project);
        Task<bool> RejectProjectStepAsync(Domain.Entities.ProjectApprovalStep project);
    }
}
