using Application.Services.ProposalService;
using Application.Services.ProposalService.ProposalDtos;


namespace Application.Interfaces.ProjectProposal
{
    public interface IProjectProposalService
    {
        Task<ProposalResponse> CreateProjectProposalAsync(ProposalRequest command);
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUser(int idUser);
        Task<bool> UpdateProposalAsync(Domain.Entities.ProjectProposal projectProposal);
    }
}
