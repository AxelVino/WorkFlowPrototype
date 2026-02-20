using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalDtos;
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

        public async Task<List<ProjectApprovalStep>> AddRangeAsync(List<ProjectApprovalStep> steps)
        {
            await _dbContext.AddRangeAsync(steps);
            await _dbContext.SaveChangesAsync();

            return steps;
        }

        public async Task<List<ProjectApprovalStep>> GetAllFiltredAsync(ProposalFilterRequest request)
        {
            var query = _dbContext.ProjectApprovalStep
                .Include(a => a.ProjectProposalObject)
                    .ThenInclude(p => p.ApprovalStatusObject)
                .Include(a => a.ProjectProposalObject)
                    .ThenInclude(p => p.UserObject)
                .Include(a => a.ProjectProposalObject)
                    .ThenInclude(p => p.AreaObject)
                .Include(a => a.ProjectProposalObject)
                    .ThenInclude(p => p.ProjectTypeObject)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(a => a.ProjectProposalObject.Title.ToLower().Contains(request.Title.ToLower()));
            }
            if (request.Status.HasValue)
            {
                query = query.Where(a => a.ProjectProposalObject.ApprovalStatusObject.Id == request.Status);
            }
            if (request.Applicant.HasValue)
            {
                if (request.ApprovalUser.HasValue)
                {
                    // Approval Mode: Exclude projects created by the approver (to avoid self-approval)
                    query = query.Where(a => a.ProjectProposalObject.UserObject.Id != request.Applicant);
                }
                else
                {
                    // Standard Mode: Filter by specific applicant
                    query = query.Where(a => a.ProjectProposalObject.UserObject.Id == request.Applicant);
                }
            }

            var steps = await query.ToListAsync();

            var activeSteps = steps
                .GroupBy(s => s.ProjectProposalId)
                .Select(g =>
                {
                    var active = g.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.Status != 2 && s.Status != 3);
                    if (active != null) return active;

                    return g.OrderByDescending(s => s.StepOrder).First();
                })
                .ToList();

            if (request.ApprovalUser.HasValue)
            {
                activeSteps = activeSteps
                    .Where(s => s.Status != 2 && s.Status != 3) 
                    .Where(s => s.ApproverRoleId == request.ApprovalUser)
                    .ToList();
            }

            return activeSteps!;
        }

        public async Task<List<ProjectApprovalStep>> GetProjectStepsById(Guid id)
        {
            return await _dbContext.ProjectApprovalStep
                .Where(p => p.ProjectProposalId == id)
                .Include(p => p.ProjectProposalObject)
                .Include(p => p.UserObject)
                    .ThenInclude(p => p.ApproverRoleObject)
                .Include(p => p.ApproverRoleObject)
                .Include(p => p.ApprovalStatusObject)
                .OrderBy(p => p.StepOrder) 
                .ToListAsync();
        }

        public async Task<List<ProjectApprovalStep>> UpdateProject(ProjectApprovalStep project)
        {
            _dbContext.ProjectApprovalStep.Update(project);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.ProjectApprovalStep
                .Where(p => p.ProjectProposalId == project.ProjectProposalId)
                .Include(p => p.ProjectProposalObject)
                    .ThenInclude(p => p.AreaObject)
                .Include(p => p.ProjectProposalObject)
                    .ThenInclude(p => p.ProjectTypeObject)
                .Include(p => p.ProjectProposalObject)
                    .ThenInclude(p => p.ApprovalStatusObject)
                .Include(p => p.ProjectProposalObject)
                    .ThenInclude(p => p.UserObject)
                .Include(p => p.UserObject)
                    .ThenInclude(p => p.ApproverRoleObject)
                .Include(p => p.ApproverRoleObject)
                .Include(p => p.ApprovalStatusObject)
                .OrderBy(p => p.StepOrder)
                .ToListAsync();
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
