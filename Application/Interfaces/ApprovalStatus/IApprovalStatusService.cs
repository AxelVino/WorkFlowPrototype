using Application.Services.ApprovalStatusService.StatusDtos;

namespace Application.Interfaces.ApprovalStatus
{
    public interface IApprovalStatusService
    {
        Task<Domain.Entities.ApprovalStatus> GetStatusByIdAsync(int id);
        Task<List<ApprovalStatusResponse>> GetAllApprovalStatus();
    }
}
