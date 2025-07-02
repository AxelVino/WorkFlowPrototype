using Application.Services.ProposalService.ProposalCommands;
using Application.Interfaces.Repository;
using MediatR;
using Domain.Entities;

namespace Application.Services.ProposalService.ProposalHandlers
{
    public class CreateProjectProposalHandler : IRequestHandler<CreateProjectProposalCommand, ProjectProposal>
    {
        private readonly IRepository<ProjectProposal> _repository;

        public CreateProjectProposalHandler(IRepository<ProjectProposal> repository)
        {
            _repository = repository;
        }

        public async Task<ProjectProposal> Handle(CreateProjectProposalCommand request, CancellationToken cancellationToken)
        {
            var projectProposal = new ProjectProposal
            {
                Title = request.Title,
                Description = request.Description,
                Area = request.Area,
                AreaObject = request.AreaObject!,
                Type = request.Type,
                ProjectTypeObject = request.ProjectTypeObject!,
                EstimatedAmount = request.EstimatedAmount,
                EstimatedDuration = request.EstimatedDuration, 
                Status = request.Status,
                ApprovalStatusObject = request.ApprovalStatusObject!,
                CreateAt = request.CreateAt,
                CreateBy = request.CreateBy,
                UserObject = request.UserObject!, 
            };

            await _repository.AddAsync(projectProposal);
            await _repository.SaveChangesAsync();

            return projectProposal;
        }
    }
}
