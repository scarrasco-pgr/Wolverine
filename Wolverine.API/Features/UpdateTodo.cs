using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{
    public record UpdateTodo(Guid TodoId, string Description, bool Completed);
    public record TodoUpdated(Guid Id, string Description, bool Completed);
    public static class UpdateTodoEndpoint
    {
        [WolverinePut("/todos")]
        public static (IResult, TodoUpdated) UpdateTodo(UpdateTodo command, [ReadAggregate] Todo _)
        {
            var updatedTodo = new TodoUpdated(Guid.CreateVersion7(), command.Description, command.Completed);
            return (
                Results.NoContent(),
                updatedTodo
            );
        }
    }
}
