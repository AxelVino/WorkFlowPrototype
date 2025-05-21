using Domain.Entities;

namespace Application.Services.UserService.UserDtos
{
    public class UserResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required ApproverRole Role { get; set; }
    }
}
