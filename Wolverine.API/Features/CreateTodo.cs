using Events;
using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{
    public record TodoCreationResponse(Guid Id)
    : CreationResponse("/todos/" + Id);

    public record CreateTodo(string Description);
    public static class CreateTodoEndpoint
    {
        [WolverinePost("/todos")]
        public static (TodoCreationResponse, IStartStream) CreateTodo(CreateTodo command)
        {
            var todoCreated = new TodoCreated(Guid.CreateVersion7(), command.Description, false);
            return (
                new TodoCreationResponse(todoCreated.Id),
                MartenOps.StartStream<Todo>(todoCreated.Id, todoCreated)
            );
        }
    }
}
