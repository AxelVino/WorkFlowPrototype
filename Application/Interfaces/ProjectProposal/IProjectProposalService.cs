using Application.Services.ProposalService.ProposalDtos;


namespace Application.Interfaces.ProjectProposal
{
    public interface IProjectProposalService
    {
        Task<Guid> CreateProjectProposalAsync(ProposalRequest command);
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUser(int idUser);
        Task<bool> UpdateProposalAsync(Domain.Entities.ProjectProposal projectProposal);
    }
}
