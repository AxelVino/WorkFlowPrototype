using Application.Responses;
namespace Application.Interfaces.Area
{
    public interface IAreaService
    {
        Task<Domain.Entities.Area> GetAreaByIdAsync(int id);
        Task<List<GenericResponse>> GetAllAreasAsync();
    }
}
