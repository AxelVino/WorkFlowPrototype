using Application.Interfaces.Repository;
using Application.Services.ProposalService.ProposalQuerys;
using MediatR;

namespace Application.Services.ProposalService.ProposalHandlers
{
    public class GetAllTitleProject : IRequestHandler<GetAllTitleProjectInUse, List<string>>
    {
        private readonly IProjectProposalRepository _repository;

        public GetAllTitleProject(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<string>> Handle(GetAllTitleProjectInUse request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllTitles();

        }
    }
}
