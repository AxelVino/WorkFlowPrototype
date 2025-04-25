using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectProposalRepository : IProjectProposalRepository
    {
        private readonly DbContextApprover _dbContext;

        public ProjectProposalRepository(DbContextApprover dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ProjectProposal>> GetAllProposalByUserAsync(int userId)
        {
            return await _dbContext.ProjectProposal
            .Where(p => p.CreateBy == userId)
            .Include(p => p.AreaObject)
            .Include(p => p.ProjectTypeObject)
            .Include(p => p.ApprovalStatusObject)
            .ToListAsync();
        }

        public async  Task<bool> UpdateProposalAsync(ProjectProposal project)
        {
            _dbContext.ProjectProposal.Update(project);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
