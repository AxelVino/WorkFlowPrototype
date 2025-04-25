namespace Application.Interfaces.Area
{
    public interface IAreaService
    {
        Task<Domain.Entities.Area?> GetAreaByIdAsync(int id);
    }
}
