namespace Application.Responses
{
    public class Project
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required double Amount { get; set; }
        public required int Duration { get; set; }
        public required GenericResponse Area { get; set; }
        public required GenericResponse Status { get; set; }
        public required GenericResponse Type { get; set; }
        public required Users User { get; set; }
        public List<ApprovalStep>? Steps { get; set; }
    }
}
