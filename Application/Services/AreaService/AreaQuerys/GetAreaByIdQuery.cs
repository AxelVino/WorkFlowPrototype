using MediatR;
namespace Application.Services.AreaService.AreaQuerys
{
    public record GetAreaByIdQuery(int Id) : IRequest<Domain.Entities.Area>;
}
