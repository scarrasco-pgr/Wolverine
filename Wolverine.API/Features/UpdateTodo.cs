using Events;
using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{
    public record UpdateTodo(string Description, bool Completed);

    public static class UpdateTodoEndpoint
    {
        [WolverinePut("/todos/{todoId}")]
        public static (IResult, TodoUpdated) UpdateTodo(UpdateTodo command, [WriteAggregate] Todo todo) =>
            (
                Results.NoContent(),
                new TodoUpdated(todo.Id, command.Description, command.Completed)
            );
    }
}
