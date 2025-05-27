using Application.Services.ProposalService.ProposalDtos;

namespace Application.Interfaces.Repository
{
    public interface IProjectProposalRepository
    {
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUserAsync(int userId);
        Task<Domain.Entities.ProjectProposal> UpdateProposalAsync(Domain.Entities.ProjectProposal request);

        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalProjects(ProposalFilterRequest request);
        Task<Domain.Entities.ProjectProposal> GetProjectProposalById(Guid id);
    }
}
