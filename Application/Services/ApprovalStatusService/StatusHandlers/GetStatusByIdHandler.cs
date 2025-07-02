using Application.Exceptions;
using Application.Interfaces.Repository;
using Application.Services.ApprovalStatusService.StatusQuerys;
using MediatR;

namespace Application.Services.ApprovalStatusService.StatusHandlers
{
    public class GetStatusByIdHandler: IRequestHandler<GetStatusByIdQuery, Domain.Entities.ApprovalStatus>
    {
        private readonly IRepository<Domain.Entities.ApprovalStatus> _repository;

        public GetStatusByIdHandler(IRepository<Domain.Entities.ApprovalStatus> repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.ApprovalStatus> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var status = await _repository.GetByIdAsync(request.Id);
            return status is null ? throw new ExceptionBadRequest($"The status with ID({request.Id}) was not found.") : status;
        }
    }
}
