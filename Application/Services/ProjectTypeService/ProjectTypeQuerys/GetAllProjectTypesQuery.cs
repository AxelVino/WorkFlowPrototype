using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectTypeService.ProjectTypeQuerys
{
    public record GetAllProjectTypesQuery() : IRequest<List<ProjectType>>;
}
