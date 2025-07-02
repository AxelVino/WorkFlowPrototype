using Application.Services.UserService.UserCommands;
using Application.Responses;

namespace Application.Interfaces.User
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserCommand command);
        Task<Domain.Entities.User?> GetUserByIdAsync(int id);
        Task<List<Users>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(UpdateUserCommand command);
        Task<bool> DeleteUserAsync(int id);
    }
}
