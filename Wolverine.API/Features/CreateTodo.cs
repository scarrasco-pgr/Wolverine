using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{
    public record TodoCreationResponse(Guid Id)
    : CreationResponse("/todos/" + Id);

    public record CreateTodo(string Description);
    public record TodoCreated(Guid Id, string Description, bool Completed);
    public static class CreateTodoEndpoint
    {
        [WolverinePost("/todos")]
        public static (TodoCreationResponse, IStartStream, TodoCreated) CreateTodo(CreateTodo command)
        {
            var todoCreated = new TodoCreated(Guid.CreateVersion7(), command.Description, false);
            return (
                new TodoCreationResponse(todoCreated.Id),
                MartenOps.StartStream<Todo>(todoCreated.Id, todoCreated),
                todoCreated
            );
        }
    }
}
