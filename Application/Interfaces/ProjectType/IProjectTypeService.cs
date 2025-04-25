namespace Application.Interfaces.ProjectType
{
    public interface IProjectTypeService
    {
        Task<Domain.Entities.ProjectType> GetTypeByIdAsync(int id);
    }
}
