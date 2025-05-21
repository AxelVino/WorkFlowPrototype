using Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos;
using Domain.Entities;

namespace Application.Services.ProposalService.ProposalDtos
{
    public class ProposalResponse
    {
        public required Guid Id {  get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; } 
        public required decimal Amount { get; set; }
        public required int Duration { get; set;}
        public required Area Area { get; set; }
        public required ApprovalStatus Status { get; set; }
        public required ProjectType Type { get; set; }
        public required List<ApprovalStepResponse> Steps { get; set; }

    }
}
