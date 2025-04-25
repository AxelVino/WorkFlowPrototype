using Application.Interfaces.Repository;
using Application.Services.ApprovalStatusService.StatusQuerys;
using Application.Services.ProjectTypeService.ProjectTypeQuerys;
using MediatR;


namespace Application.Services.ProjectTypeService.ProjectTypeHandlers
{
    public class GetProjectTypeByIdHandler: IRequestHandler<GetProjectTypeByIdQuery, Domain.Entities.ProjectType>
    {
        private readonly IRepository<Domain.Entities.ProjectType> _userRepository;

        public GetProjectTypeByIdHandler(IRepository<Domain.Entities.ProjectType> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Domain.Entities.ProjectType> Handle(GetProjectTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var projectType = await _userRepository.GetByIdAsync(request.Id);
            return projectType is null ? throw new Exception($"The Project type with ID({request.Id}) was not found.") : projectType;
        }
    }
}
