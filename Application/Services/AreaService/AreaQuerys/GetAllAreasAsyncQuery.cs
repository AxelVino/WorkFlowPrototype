using Domain.Entities;
using MediatR;

namespace Application.Services.AreaService.AreaQuerys
{
    public record GetAllAreasAsyncQuery () : IRequest<List<Area>>;
}
