using Application.Interfaces.User;
using Application.Services.UserService.UserCommands; 
using Application.Services.UserService.UserQuerys;
using MediatR;

namespace Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Task<int> CreateUserAsync(CreateUserCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Domain.Entities.User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.User?> GetUserByIdAsync(int id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }

        public Task<bool> UpdateUserAsync(UpdateUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
