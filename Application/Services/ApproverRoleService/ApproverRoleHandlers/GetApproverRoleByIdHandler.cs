using Application.Interfaces.Repository;
using Application.Services.ApproverRoleService.ApproverRoleQuerys;
using Application.Services.ProjectTypeService.ProjectTypeQuerys;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApproverRoleService.ApproverRoleHandlers
{
    public class GetApproverRoleByIdHandler : IRequestHandler<GetApproverRoleByIdQuery, ApproverRole>
    {
        private readonly IRepository<ApproverRole> _userRepository;

        public GetApproverRoleByIdHandler(IRepository<ApproverRole> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApproverRole> Handle(GetApproverRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var approverRole = await _userRepository.GetByIdAsync(request.Id);
            return approverRole is null ? throw new Exception($"The approver role with ID({request.Id}) was not found.") : approverRole;
        }
    }
}
