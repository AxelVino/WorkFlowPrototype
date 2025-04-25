using Application.Interfaces.ApprovalStatus;
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
        public async Task<ApprovalStatus> GetStatusByIdAsync(int id)
        {
            return await _mediator.Send(new GetStatusByIdQuery(id));
        }
    }
}
