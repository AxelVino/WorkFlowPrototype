using Application.Interfaces.Repository;
using Application.Services.ApprovalStatusService.StatusQuerys;
using Application.Services.AreaService.AreaQuerys;
using MediatR;

namespace Application.Services.ApprovalStatusService.StatusHandlers
{
    public class GetStatusByIdHandler: IRequestHandler<GetStatusByIdQuery, Domain.Entities.ApprovalStatus>
    {
        private readonly IRepository<Domain.Entities.ApprovalStatus> _userRepository;

        public GetStatusByIdHandler(IRepository<Domain.Entities.ApprovalStatus> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Domain.Entities.ApprovalStatus> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var status = await _userRepository.GetByIdAsync(request.Id);
            return status is null ? throw new Exception($"The status with ID({request.Id}) was not found.") : status;
        }
    }
}
