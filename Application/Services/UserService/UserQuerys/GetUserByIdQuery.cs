using MediatR;

namespace Application.Services.UserService.UserQuerys
{
    public record GetUserByIdQuery(int Id) : IRequest<Domain.Entities.User>;
}
