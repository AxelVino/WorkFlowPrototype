using Application.Services.ProjectTypeService.ProjectTypeDtos;

namespace Application.Interfaces.ProjectType
{
    public interface IProjectTypeService
    {
        Task<Domain.Entities.ProjectType> GetTypeByIdAsync(int id);
        Task<List<ProjectTypeResponse>> GetAllProjectTypes();
    }
}
