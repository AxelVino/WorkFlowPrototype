using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalDtos;
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

        public async Task<List<ProjectProposal>> GetAllProposalProjects(ProposalFilterRequest request)
        {
            var query = _dbContext.ProjectProposal.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(a => a.Title.Contains(request.Title));
            }
            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status);
            }
            if (request.Applicant.HasValue)
            {
                query = query.Where(a => a.CreateBy == request.Applicant);
            }

            return await query.ToListAsync();
        }

        public Task<List<string>> GetAllTitles()
        {
            return _dbContext.ProjectProposal
                .Select(p => p.Title)
                .ToListAsync();
        }

        public async Task<ProjectProposal> GetProjectProposalById(Guid id)
        {
            return await _dbContext.ProjectProposal
                .Where(p => p.Id == id)
                .Include(p => p.AreaObject)
                .Include(p => p.ProjectTypeObject)
                .Include(p => p.ApprovalStatusObject)
                .Include(p => p.UserObject)
                .ThenInclude(p => p.ApproverRoleObject)
                .FirstOrDefaultAsync();
        }

        public async Task<ProjectProposal> UpdateProposalAsync(ProjectProposal request)
        {
            _dbContext.ProjectProposal.Update(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }
    }
}
