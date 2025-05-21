using Application.Interfaces.Repository;
using Application.Services.ApprovalStatusService.StatusQuerys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApprovalStatusService.StatusHandlers
{
    public class GetAllApprovalStatusHandler : IRequestHandler<GetAllApprovalStatusQuery, List<Domain.Entities.ApprovalStatus>>
    {
        private readonly IRepository<Domain.Entities.ApprovalStatus> _repository;

        public GetAllApprovalStatusHandler(IRepository<Domain.Entities.ApprovalStatus> repository)
        {
            _repository = repository;
        }

        public async Task<List<Domain.Entities.ApprovalStatus>> Handle(GetAllApprovalStatusQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
