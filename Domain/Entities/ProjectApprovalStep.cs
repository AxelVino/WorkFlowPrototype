namespace Domain.Entities
{
    public class ProjectApprovalStep
    {
        public long Id { get; set; }
        public required Guid ProjectProposalId { get; set; }
        public required ProjectProposal ProjectProposalObject { get; set; }
        public int? ApproverUserId { get; set; }
        public User? UserObject { get; set; }
        public required int  ApproverRoleId { get; set; }
        public required ApproverRole ApproverRoleObject { get; set; }
        public required int Status { get; set; }
        public required ApprovalStatus ApprovalStatusObject { get; set; }
        public required int StepOrder {  get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
    }
}
