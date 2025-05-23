using Application.Services.AreaService.AreaDtos;

namespace Application.Interfaces.Area
{
    public interface IAreaService
    {
        Task<Domain.Entities.Area> GetAreaByIdAsync(int id);
        Task<List<AreaResponse>> GetAllAreasAsync();
    }
}
