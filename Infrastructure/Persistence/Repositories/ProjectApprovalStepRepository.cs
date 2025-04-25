using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectApprovalStepRepository : IProjectApprovalStepRepository
    {
        private readonly DbContextApprover _dbContext;

        public ProjectApprovalStepRepository(DbContextApprover dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddRangeAsync(List<ProjectApprovalStep> steps)
        {
            await _dbContext.AddRangeAsync(steps);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProjectApprovalStep>> GetAllFiltredAsync(int approverRoleId)
        {
            var allSteps = await _dbContext.ProjectApprovalStep
                .Include(p => p.ProjectProposalObject)
                .ThenInclude(p => p.ProjectTypeObject)
                .Include(p => p.ProjectProposalObject)
                .ThenInclude(p => p.UserObject)
                .Include(p => p.ProjectProposalObject)
                .ThenInclude(p => p.AreaObject)
                .Include(p => p.ApprovalStatusObject)
                .ToListAsync();

            var stepsByProposal = allSteps
                 .GroupBy(s => s.ProjectProposalId)
                 .ToDictionary(
                 g => g.Key,
                 g => g.OrderBy(s => s.StepOrder).ToList()
                 );

            var filteredSteps = allSteps
                .Where(p =>
                    p.ApproverRoleId == approverRoleId &&
                    (p.Status == 1 || p.Status == 4) &&
                    (
                        p.StepOrder == 1 || (
                            stepsByProposal.TryGetValue(p.ProjectProposalId, out var stepsOfProject) &&
                            stepsOfProject.Any(prev =>
                                prev.StepOrder == p.StepOrder - 1 &&
                                prev.Status == 2
                            )
                        )
                    )
                )
                .ToList();

            return filteredSteps;
        }

        public async Task<bool> UpdateProject(ProjectApprovalStep project)
        {
            _dbContext.ProjectApprovalStep.Update(project);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> VerifyStepsSameGuid(Guid projectGuid)
        {
            var steps = await _dbContext.ProjectApprovalStep
                .Where(s => s.ProjectProposalId == projectGuid)
                .ToListAsync();

            return steps.All(s => s.Status == 2);
        }
    }
}
