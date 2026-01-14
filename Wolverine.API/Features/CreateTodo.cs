using Marten;
using Wolverine.API.Models;
using Wolverine.Http;

namespace Wolverine.API.Features
{
    public record TodoCreationResponse(Guid Id)
    : CreationResponse("/todos/" + Id);

    public record CreateTodo(string Description);
    public record TodoCreated(Guid Id, string Description, bool Completed);
    public static class TodoCreationEndpoint
    {
        [WolverinePost("/todos")]
        public static (TodoCreationResponse, TodoCreated) CreateTodo(CreateTodo command, IDocumentSession session)
        {
            var todoCreated = new TodoCreated(Guid.NewGuid(), command.Description, false);
            session.Events.StartStream<Todo>(todoCreated.Id, todoCreated);
            return (
                new TodoCreationResponse(todoCreated.Id),
                todoCreated
            );
        }
    }
}
