using Application.Interfaces.ProjectType;
using Application.Services.ProjectTypeService.ProjectTypeQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.ProjectTypeService
{
    public class ProjectTypeService : IProjectTypeService
    {
        private readonly IMediator _mediator;
        public ProjectTypeService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ProjectType> GetTypeByIdAsync(int id)
        {
            return await _mediator.Send(new GetProjectTypeByIdQuery(id));
        }
    }
}
