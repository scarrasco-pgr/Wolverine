using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Marten;

namespace Wolverine.API.Features
{

    public static class GetTodoEndpoint
    {
        [WolverineGet("/todos/{id}")]
        public static Todo GetTodo([ReadAggregate] Todo todo) => todo;

    }
}
