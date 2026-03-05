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

        public static Update<Todo> Handle(TodoUpdated @event, [Entity] Todo todo)
        {
            todo.Description = @event.Description;
            todo.Completed = @event.Completed;
            return Storage.Update(todo);
        }

        public static Delete<Todo> Handle(TodoDeleted _, [Entity] Todo todo)
        {
            return Storage.Delete(todo);
        }
    }
}
