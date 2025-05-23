using Application.Services.ProposalService.ProposalDtos;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IProjectApprovalStepRepository
    {
        Task<List<Domain.Entities.ProjectApprovalStep>> GetAllFiltredAsync(ProposalFilterRequest request);
        Task<bool> AddRangeAsync(List<Domain.Entities.ProjectApprovalStep> steps);
        Task<bool> UpdateProject(Domain.Entities.ProjectApprovalStep project);

        Task<bool> VerifyStepsSameGuid(Guid proyectGuid);
    }
}
