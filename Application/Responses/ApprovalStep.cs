namespace Application.Responses
{
    public class ApprovalStep
    {
        public required long Id { get; set; }
        public required int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
        public required Users ApproverUser { get; set; }
        public required GenericResponse ApproverRole { get; set; }
        public required GenericResponse Status { get; set; }

    }
}
