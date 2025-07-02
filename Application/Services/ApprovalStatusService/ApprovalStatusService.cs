using Application.Exceptions;
using Application.Interfaces.ApprovalStatus;
using Application.Responses;
using Application.Services.ApprovalStatusService.StatusQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ApprovalStatusService
{
    public class ApprovalStatusService : IApprovalStatusService
    {
        private readonly IMediator _mediator;

        public ApprovalStatusService(IMediator mediator)
        {

            _mediator = mediator;

        }

        public async Task<List<GenericResponse>> GetAllApprovalStatus()
        {
            List<ApprovalStatus> list = await _mediator.Send(new GetAllApprovalStatusQuery());
            List<GenericResponse> listResponse = [];
            foreach (ApprovalStatus status in list)
            {
                GenericResponse response = new() 
                { 
                    Id = status.Id,
                    Name=status.Name 
                };
                listResponse.Add(response);
            }
            return listResponse;
        }

        public async Task<ApprovalStatus> GetStatusByIdAsync(int id)
        {
            ApprovalStatus status = await _mediator.Send(new GetStatusByIdQuery(id));
            if (status == null)
                throw new ExceptionNotFound("Status not found, please enter a valid status.");

            return status;
        }
    }
}
