using Domain.Entities;
using MediatR;

namespace Application.Services.ApproverRoleService.ApproverRoleQuerys
{
    public record GetApproverRoleByIdQuery(int Id) : IRequest<ApproverRole>;
}

