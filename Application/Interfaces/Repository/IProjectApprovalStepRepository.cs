using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IProjectApprovalStepRepository
    {
        Task<List<Domain.Entities.ProjectApprovalStep>> GetAllFiltredAsync(ProposalFilterRequest request);
        Task<List<Domain.Entities.ProjectApprovalStep>> AddRangeAsync(List<Domain.Entities.ProjectApprovalStep> steps);
        Task<List<Domain.Entities.ProjectApprovalStep>> UpdateProject(Domain.Entities.ProjectApprovalStep project);
        Task<List<Domain.Entities.ProjectApprovalStep>> GetProjectStepsById(Guid id);
        Task<bool> VerifyStepsSameGuid(Guid proyectGuid);
    }
}
