using Domain.Entities;
using MediatR;

namespace Application.Services.ProposalService.ProposalCommands
{
    public class CreateProjectProposalCommand : IRequest<ProjectProposal>
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Area { get; set; }
        public Area? AreaObject { get; set; }
        public required int Type { get; set; }
        public ProjectType? ProjectTypeObject { get; set; }
        public required decimal EstimatedAmount { get; set; }
        public required int EstimatedDuration { get; set; }
        public required int Status { get; set; }
        public ApprovalStatus? ApprovalStatusObject { get; set; }
        public required DateTime CreateAt { get; set; }
        public required int CreateBy { get; set; }
        public User? UserObject { get; set; }
    }
}
