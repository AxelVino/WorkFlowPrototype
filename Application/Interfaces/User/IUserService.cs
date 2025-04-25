using Application.Services.UserService.UserDtos;
using Application.Services.UserService.UserCommands;

namespace Application.Interfaces.User
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserCommand command);
        Task<Domain.Entities.User?> GetUserByIdAsync(int id);
        Task<List<Domain.Entities.User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(UpdateUserCommand command);
        Task<bool> DeleteUserAsync(int id);
    }
}
