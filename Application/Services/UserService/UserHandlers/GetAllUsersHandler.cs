using Application.Interfaces.Repository;
using Application.Services.UserService.UserQuerys;
using MediatR;

namespace Application.Services.UserService.UserHandlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Domain.Entities.User>>
    {
        private readonly IRepository<Domain.Entities.User> _repository;

        public GetAllUsersHandler(IRepository<Domain.Entities.User> repository)
        {
            _repository = repository;
        }

        public async Task<List<Domain.Entities.User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
