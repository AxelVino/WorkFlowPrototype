using MediatR;
namespace Application.Services.ProjectTypeService.ProjectTypeQuerys
{
    public record GetProjectTypeByIdQuery(int Id) : IRequest<Domain.Entities.ProjectType>;
}
