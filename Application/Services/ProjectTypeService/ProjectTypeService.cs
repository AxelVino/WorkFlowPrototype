using Application.Interfaces.ProjectType;
using Application.Responses;
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

        public async Task<List<GenericResponse>> GetAllProjectTypes()
        {
            List<ProjectType> list = await _mediator.Send(new GetAllProjectTypesQuery());
            List <GenericResponse> listResponse = [];
            foreach (ProjectType projectType in list)
            {
                GenericResponse response = new() 
                {
                    Id = projectType.Id, 
                    Name = projectType.Name 
                };
                listResponse.Add(response);
            }
            return listResponse;
        }
        public async Task<ProjectType> GetTypeByIdAsync(int id)
        {
            return await _mediator.Send(new GetProjectTypeByIdQuery(id));
        }
    }
}
