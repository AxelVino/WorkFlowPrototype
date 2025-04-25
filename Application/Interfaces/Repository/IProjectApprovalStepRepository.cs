namespace Application.Interfaces.Repository
{
    public interface IProjectApprovalStepRepository
    {
        Task<List<Domain.Entities.ProjectApprovalStep>> GetAllFiltredAsync(int approverRoleId);
        Task<bool> AddRangeAsync(List<Domain.Entities.ProjectApprovalStep> steps);
        Task<bool> UpdateProject(Domain.Entities.ProjectApprovalStep project);

        Task<bool> VerifyStepsSameGuid(Guid proyectGuid);
    }
}
