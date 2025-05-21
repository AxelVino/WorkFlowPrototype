using Application.Interfaces.Repository;
using Application.Services.ProjectTypeService.ProjectTypeQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectTypeService.ProjectTypeHandlers
{
    public class GetAllProjectTypesHandler : IRequestHandler<GetAllProjectTypesQuery, List<ProjectType>>
    {
        private readonly IRepository<ProjectType> _repository;

        public GetAllProjectTypesHandler(IRepository<ProjectType> repository)
        {
            _repository = repository;
        }

        public async Task<List<ProjectType>> Handle(GetAllProjectTypesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
