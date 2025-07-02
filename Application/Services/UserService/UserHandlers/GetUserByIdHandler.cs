using Application.Interfaces.Repository;
using MediatR;
using Application.Services.UserService.UserQuerys;
using Application.Exceptions;

namespace Application.Services.UserService.UserHandlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Domain.Entities.User>
    {
        private readonly IRepository<Domain.Entities.User> _userRepository;

        public GetUserByIdHandler(IRepository<Domain.Entities.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Domain.Entities.User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            return user is null ? throw new ExceptionBadRequest($"The user with id {request.Id} was not found.") : user;
        }
    }
}
