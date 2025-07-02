using Application.Exceptions;
using Application.Interfaces.ApproverRole;
using Application.Responses;
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

        public async Task<List<GenericResponse>> GetAllApproverRoles()
        {
            List<ApproverRole> list = await _mediator.Send(new GetAllApproverRoleQuery());
            List<GenericResponse> listResponse = [];
            foreach (ApproverRole role in list)
            {
                GenericResponse response = new() 
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
            ApproverRole role =  await _mediator.Send(new GetApproverRoleByIdQuery(id));

            if (role == null)
                throw new ExceptionNotFound("Role not found, please enter a valid role.");

            return role;
        }
    }
}
