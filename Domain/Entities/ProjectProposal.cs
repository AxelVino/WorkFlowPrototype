namespace Domain.Entities
{
    public class ProjectProposal
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Area { get; set; }
        public required Area AreaObject { get; set; }
        public required int Type { get; set; }
        public required ProjectType ProjectTypeObject { get; set; }
        public required decimal EstimatedAmount { get; set; }
        public required int EstimatedDuration { get; set;}
        public required int Status { get; set;}
        public required ApprovalStatus ApprovalStatusObject { get; set;}
        public required DateTime CreateAt { get; set; }
        public required int CreateBy { get; set; }
        public required User UserObject { get; set; }

    }
}
