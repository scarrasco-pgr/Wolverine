namespace Search.API.Data.Models
{
    public sealed class Todo
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public bool Completed { get; set; }
    }
}
