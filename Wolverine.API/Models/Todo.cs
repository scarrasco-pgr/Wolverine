using Events;

namespace Wolverine.API.Models;

public class Todo
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }

    public void Apply(TodoCreated @event)
    {
        Id = @event.Id;
        Description = @event.Description;
        Completed = @event.Completed;
    }

    public void Apply(TodoUpdated @event)
    {
        Description = @event.Description;
        Completed = @event.Completed;
    }

}
