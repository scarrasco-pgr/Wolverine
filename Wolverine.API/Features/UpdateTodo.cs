using Marten;
using Wolverine.Http;

namespace Wolverine.API.Features
{
    public record UpdateTodo(string Description, bool Completed);
    public record TodoUpdated(Guid Id, string Description, bool Completed);
    public static class UpdateTodoEndpoint
    {
        [WolverinePut("/todos/{Id}")]
        public static (IResult, TodoUpdated) UpdateTodo(Guid Id, UpdateTodo command, IDocumentSession session)
        {
            var updatedTodo = new TodoUpdated(Guid.CreateVersion7(), command.Description, command.Completed);
            session.Events.Append(Id, updatedTodo);
            return (
                Results.NoContent(),
                updatedTodo
            );
        }
    }
}
