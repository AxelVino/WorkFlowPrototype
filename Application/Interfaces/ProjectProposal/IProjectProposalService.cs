using Application.Services.ProposalService.ProposalCommands;

namespace Application.Interfaces.ProjectProposal
{
    public interface IProjectProposalService
    {
        Task<Guid> CreateProjectProposalAsync(CreateProjectProposalCommand command);
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUser(int idUser);
        Task<bool> UpdateProposalAsync(Domain.Entities.ProjectProposal projectProposal);
    }
}
