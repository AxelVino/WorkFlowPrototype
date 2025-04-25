using Application.Interfaces.Repository;
using Application.Services.ProjectApprovalStepService.ProjectApprovalStepQuerys;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;


namespace Application.Services.ProjectApprovalStepService.ProjectApprovalStepHandlers
{
    public class GetListProjectsByIdHandler : IRequestHandler<GetListProjectsByIdQuery, List<ProjectApprovalStep>>
    {
        private readonly IProjectApprovalStepRepository _repository;

        public GetListProjectsByIdHandler(IProjectApprovalStepRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<List<ProjectApprovalStep>> Handle(GetListProjectsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllFiltredAsync(request.approverRoleId);
        }
    }
}
