using Application.Services.UserService.UserDtos;
using Domain.Entities;

namespace Application.Services.ProjectApprovalStepService.ProjectApproalStepDtos
{
    public class ApprovalStepResponse
    {
        public required int Id { get; set; }
        public required int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }

        public required ApproverUserResponse ApproverUser { get; set; }
        public required ApproverRole ApproverRole { get; set; }
        public required ApprovalStatus Status { get; set; }

    }
}
