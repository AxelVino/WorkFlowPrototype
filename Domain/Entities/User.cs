namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required int Role { get; set; }
        public ApproverRole? ApproverRoleObject { get; set; }

    }
}
