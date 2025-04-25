using MediatR;

namespace Application.Services.UserService.UserCommands
{
    public class CreateUserCommand : IRequest<int> 
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int Role { get; set; }
    }
}
