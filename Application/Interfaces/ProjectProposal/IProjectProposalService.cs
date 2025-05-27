using Application.Services.ProposalService.ProposalDtos;


namespace Application.Interfaces.ProjectProposal
{
    public interface IProjectProposalService
    {
        Task<ProposalResponse> CreateProjectProposalAsync(ProposalRequest command);
        Task<List<Domain.Entities.ProjectProposal>> GetAllProposalByUser(int idUser);
        Task<ProposalResponse> GetProjectById(Guid id);
        Task<ProposalResponse> UpdateProposalAsync(Guid id ,ProposalUpdateRequest request);
    }
}
