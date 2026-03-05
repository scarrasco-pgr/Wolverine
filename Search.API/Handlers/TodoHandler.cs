using Events;
using Search.API.Data.Models;
using Wolverine.Persistence;

namespace Search.API.Handlers
{
    public static class TodoHandler
    {
        public static Insert<Todo> Handle(TodoCreated @event) => Storage.Insert(new Todo
        {
            Id = @event.Id,
            Description = @event.Description,
            Completed = @event.Completed,
        });
    }
}
