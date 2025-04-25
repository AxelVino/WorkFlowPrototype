using Application.Interfaces.Area;
using Application.Services.AreaService.AreaQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.AreaService
{
    public class AreaService : IAreaService
    {
        private readonly IMediator _mediator;

        public AreaService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Area?> GetAreaByIdAsync(int id)
        {
            return await _mediator.Send(new GetAreaByIdQuery(id));
        }
    }
}
