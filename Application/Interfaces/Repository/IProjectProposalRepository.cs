using Application.Services.ProposalService.ProposalDtos;

namespace Application.Interfaces.Repository
{
    public interface IProjectProposalRepository
    {
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUserAsync(int userId);
        Task<bool> UpdateProposalAsync(Domain.Entities.ProjectProposal proyect);

        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalProjects(ProposalFilterRequest request);
    }
}
