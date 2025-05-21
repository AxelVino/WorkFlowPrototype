using Domain.Entities;
using MediatR;

namespace Application.Services.ApproverRoleService.ApproverRoleQuerys
{
    public record GetAllApproverRoleQuery() : IRequest<List<ApproverRole>>;
}
