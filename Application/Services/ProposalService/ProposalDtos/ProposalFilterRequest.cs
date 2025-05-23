namespace Application.Services.ProposalService.ProposalDtos
{
    public class ProposalFilterRequest
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public int? Applicant {  get; set; }
        public int? ApprovalUser { get; set; }
    }
}
