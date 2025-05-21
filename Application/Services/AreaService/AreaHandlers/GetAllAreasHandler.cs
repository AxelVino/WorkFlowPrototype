using Application.Interfaces.Repository;
using Application.Services.AreaService.AreaQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.AreaService.AreaHandlers
{
    public class GetAllAreasHandler : IRequestHandler<GetAllAreasAsyncQuery, List<Area>>
    {
        private readonly IRepository<Area> _repository;

        public GetAllAreasHandler(IRepository<Area> repository)
        {
            _repository = repository;
        }

        public async Task<List<Area>> Handle(GetAllAreasAsyncQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
