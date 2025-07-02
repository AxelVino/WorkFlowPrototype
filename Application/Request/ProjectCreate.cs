using Domain.Entities;

namespace Application.Request
{
    public class ProjectCreate
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Amount { get; set; }
        public required int Duration { get; set; }
        public required int Area { get; set; }
        public required int User { get; set; }
        public required int Type { get; set; }

    }
}
