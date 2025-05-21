using Application.Interfaces.Repository;
using Application.Services.ProjectTypeService.ProjectTypeQuerys;
using MediatR;


namespace Application.Services.ProjectTypeService.ProjectTypeHandlers
{
    public class GetProjectTypeByIdHandler: IRequestHandler<GetProjectTypeByIdQuery, Domain.Entities.ProjectType>
    {
        private readonly IRepository<Domain.Entities.ProjectType> _repository;

        public GetProjectTypeByIdHandler(IRepository<Domain.Entities.ProjectType> repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.ProjectType> Handle(GetProjectTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var projectType = await _repository.GetByIdAsync(request.Id);
            return projectType is null ? throw new Exception($"The Project type with ID({request.Id}) was not found.") : projectType;
        }
    }
}
