using MediatR;

namespace Application.Services.UserService.UserQuerys
{
    public record GetAllUsersQuery() : IRequest<List<Domain.Entities.User>>;
}
