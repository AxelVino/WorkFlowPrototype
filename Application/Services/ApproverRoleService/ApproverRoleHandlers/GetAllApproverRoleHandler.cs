using Application.Interfaces.Repository;
using Application.Services.ApproverRoleService.ApproverRoleQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ApproverRoleService.ApproverRoleHandlers
{
    public class GetAllApproverRoleHandler : IRequestHandler<GetAllApproverRoleQuery, List<ApproverRole>>
    {
        private readonly IRepository<ApproverRole> _repository;

        public GetAllApproverRoleHandler(IRepository<ApproverRole> repository)
        {
            _repository = repository;
        }

        public async Task<List<ApproverRole>> Handle(GetAllApproverRoleQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
