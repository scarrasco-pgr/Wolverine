using Marten;
using Wolverine.Http;

namespace Wolverine.API.Features
{
    public record TodoUpdatedResponse(Guid Id)
    : CreationResponse("/todos/" + Id);

    public record UpdateTodo(string Description, bool Completed);
    public record TodoUpdated(string Description, bool Completed);
    public static class TodoCompletionEndpoint
    {
        [WolverinePut("/todos/{id}")]
        public static (TodoUpdatedResponse, TodoUpdated) UpdateTodo(Guid Id, UpdateTodo command, IDocumentSession session)
        {
            var updatedTodo = new TodoUpdated(command.Description, command.Completed);
            session.Events.Append(Id, updatedTodo);
            return (
                new TodoUpdatedResponse(Id),
                updatedTodo
            );
        }
    }
}
