using Application.Exceptions;
using Application.Interfaces.Repository;
using Application.Services.AreaService.AreaQuerys;
using MediatR;

namespace Application.Services.AreaService.AreaHandlers
{
    public class GetAreaByIdHandler: IRequestHandler<GetAreaByIdQuery, Domain.Entities.Area>
    {
        private readonly IRepository<Domain.Entities.Area> _repository;

        public GetAreaByIdHandler(IRepository<Domain.Entities.Area> repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.Area> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
        {
            var area = await _repository.GetByIdAsync(request.Id);
            return area is null ? throw new ExceptionBadRequest($"The area with ID({request.Id}) was not found.") : area;
        }
    }
}
