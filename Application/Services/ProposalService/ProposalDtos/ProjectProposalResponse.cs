namespace Application.Services.ProposalService.ProposalDtos
{
    public class ProjectProposalResponse
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Amount { get; set; }
        public required int Duration { get; set; }
        public required string Area { get; set; }
        public required string Status { get; set; }
        public required string Type { get; set; }

    }
}
