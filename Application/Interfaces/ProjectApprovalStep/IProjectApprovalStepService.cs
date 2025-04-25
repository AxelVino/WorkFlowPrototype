using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;

namespace Application.Interfaces.ProjectApprovalStep
{
    public interface IProjectApprovalStepService
    {
        Task<bool>CreateProjectApprovalStepAsync(IncompletedProjectDto incompleted);
        Task<List<Domain.Entities.ProjectApprovalStep>> GetListProjectsById(int approverRoleId); 
        Task<bool>ApproveProjectStepAsync(Domain.Entities.ProjectApprovalStep project);
        Task<bool> RejectProjectStepAsync(Domain.Entities.ProjectApprovalStep project);
    }
}
