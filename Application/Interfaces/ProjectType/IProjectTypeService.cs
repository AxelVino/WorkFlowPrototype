using Application.Responses;

namespace Application.Interfaces.ProjectType
{
    public interface IProjectTypeService
    {
        Task<Domain.Entities.ProjectType> GetTypeByIdAsync(int id);
        Task<List<GenericResponse>> GetAllProjectTypes();
    }
}
