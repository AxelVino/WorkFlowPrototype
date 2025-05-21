namespace Application.Services.ProposalService.ProposalDtos
{
    public class ProposalRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Amount { get; set; }
        public required int Duration { get; set; }
        public required int Area { get; set; }
        public required int Status { get; set; }
        public required int Type { get; set; }
        public required int User { get; set; }
    }
}
