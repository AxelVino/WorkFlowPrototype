using Application.Request;
using Application.Responses;
using Application.Services.ProposalService.ProposalDtos;

namespace Application.Interfaces.ProjectProposal
{
    public interface IProjectProposalService
    {
        Task<Project> CreateProjectProposalAsync(ProjectCreate command);
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUser(int idUser);
        Task<Project> GetProjectById(Guid id);
        Task<Project> UpdateProposalAsync(Guid id ,ProposalUpdateRequest request);
        Task<Project> EvaluateProject(Guid id, DecisionRequest request);
        
    }
}
