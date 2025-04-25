namespace Application.Services.UserService.UserDtos
{
    public class ResponseUserDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int Role { get; set; }

    }
}
