using Domain.Entities;


namespace Application.Responses
{
    public class Users
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required GenericResponse Role { get; set; }
    }
}
