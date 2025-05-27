namespace Application.Services.ProposalService.ProposalDtos
{
    public class ProposalUpdateRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
    }
}
