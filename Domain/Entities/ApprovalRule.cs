namespace Domain.Entities
{
    public class ApprovalRule
    {
        public long Id { get; set; }
        public required decimal MinAmount { get; set; }
        public required decimal MaxAmount { get; set; }
        public int? Area { get; set; }
        public Area? AreaObject { get; set; }
        public int? Type { get; set; }
        public ProjectType? ProjectTypeObject { get; set; }
        public required int StepOrder { get; set; }
        public required int ApproverRoleId { get; set; }
        public ApproverRole? ApproverRoleObject { get; set; }
    }
}
