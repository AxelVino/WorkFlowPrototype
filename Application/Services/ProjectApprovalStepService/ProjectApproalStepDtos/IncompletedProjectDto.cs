using Domain.Entities;

namespace Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos
{
    public class IncompletedProjectDto
    {
        public required Guid ProjectProposalId { get; set; }
        public required ProjectProposal ProjectProposalObject { get; set; }
        public required int ApproverRoleId { get; set; }
        public required int Status { get; set; }
        public required ApprovalStatus ApprovalStatusObject { get; set; }
        public required int StepOrder {  get; set; }
    }

}
