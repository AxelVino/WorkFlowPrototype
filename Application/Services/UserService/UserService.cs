using Application.Exceptions;
using Application.Interfaces.ApproverRole;
using Application.Interfaces.User;
using Application.Responses;
using Application.Services.UserService.UserCommands;
using Application.Services.UserService.UserQuerys;
using Domain.Entities;
using MediatR;

namespace Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;
        private readonly IApproverRoleService _approverRoleService;

        public UserService(IMediator mediator, IApproverRoleService approverRoleService)
        {
            _mediator = mediator;
            _approverRoleService = approverRoleService;
        }
        public Task<int> CreateUserAsync(CreateUserCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null)
                throw new ExceptionNotFound("User not found, please enter a valid user.");

            return user;
        }

        public Task<bool> UpdateUserAsync(UpdateUserCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            List<User> list = await _mediator.Send(new GetAllUsersQuery());
            List<Users> listResponse = [];
            foreach (User user in list)
            {
                ApproverRole element = await _approverRoleService.GetApproverRoleByIdAsync(user.Role);
                Users response = new() 
                { 
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = new GenericResponse() { Id = element.Id, Name= element.Name },
                };
                listResponse.Add(response);
            }
            return listResponse;
        }
    }
}
