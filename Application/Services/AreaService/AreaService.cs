using Application.Interfaces.Area;
using Application.Responses;
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

        public async Task<List<GenericResponse>> GetAllAreasAsync()
        {
            List<Area> list = await _mediator.Send(new GetAllAreasAsyncQuery());
            List<GenericResponse> listResponse = [];
            foreach (Area area in list) 
            {
                GenericResponse response = new() 
                { 
                  Id = area.Id, 
                  Name = area.Name
                };
                listResponse.Add(response);
            }
            return listResponse;
        }
        public async Task<Area> GetAreaByIdAsync(int id)
        {
            return await _mediator.Send(new GetAreaByIdQuery(id));
        }
    }
}
