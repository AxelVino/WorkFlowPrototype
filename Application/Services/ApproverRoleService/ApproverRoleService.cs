using Application.Interfaces.ApproverRole;
using Application.Services.ApproverRoleService.ApproverRoleDtos;
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

        public async Task<List<ApproverRoleResponse>> GetAllApproverRoles()
        {
            List<ApproverRole> list = await _mediator.Send(new GetAllApproverRoleQuery());
            List<ApproverRoleResponse> listResponse = [];
            foreach (ApproverRole role in list)
            {
                ApproverRoleResponse response = new() 
                { 
                    Id = role.Id, 
                    Name = role.Name 
                };
                listResponse.Add(response);
            }
            return listResponse;
        }

        public async Task<ApproverRole> GetApproverRoleByIdAsync(int id)
        {
            return await _mediator.Send(new GetApproverRoleByIdQuery(id));
        }
    }
}
