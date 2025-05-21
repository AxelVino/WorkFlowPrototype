using Application.Services.ApproverRoleService.ApproverRoleDtos;

namespace Application.Interfaces.ApproverRole
{
    public interface IApproverRoleService
    {
        Task<Domain.Entities.ApproverRole> GetApproverRoleByIdAsync(int id);
        Task<List<ApproverRoleResponse>> GetAllApproverRoles();
    }
}
