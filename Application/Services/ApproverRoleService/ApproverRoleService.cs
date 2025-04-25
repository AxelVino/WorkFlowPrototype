
using Application.Interfaces.ApproverRole;
using Application.Services.ApproverRoleService.ApproverRoleQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ApproverRoleService
{
    public class ApproverRoleService : IApproverRoleService
    {
        private readonly IMediator _mediator; 
        public ApproverRoleService(IMediator mediator) {
            _mediator = mediator;
        }
        public async Task<ApproverRole> GetApproverRoleByIdAsync(int id)
        {
            return await _mediator.Send(new GetApproverRoleByIdQuery(id));
        }
    }
}
