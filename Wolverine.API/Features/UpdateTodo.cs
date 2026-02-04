using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{
    public record UpdateTodo(string Description, bool Completed);

    public record TodoUpdated(Guid Id, string Description, bool Completed);

    public static class UpdateTodoEndpoint
    {
        [WolverinePut("/todos/{todoId}")]
        public static (IResult, TodoUpdated) UpdateTodo(UpdateTodo command, [WriteAggregate] Todo _) =>
            (
                Results.NoContent(),
                new TodoUpdated(Guid.CreateVersion7(), command.Description, command.Completed)
            );
    }
}
